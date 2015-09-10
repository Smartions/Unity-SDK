﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public static class SinoMoGoJson {
        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
		public static object SinoMoGoDeserialize(string json) {
            // save the string for debug information
            if (json == null) {
                return null;
            }

            return SinoMoGoParser.Parse(json);
        }

		sealed class SinoMoGoParser : IDisposable {
            const string WORD_BREAK = "{}[],:\"";

            public static bool IsWordBreak(char c) {
                return Char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
            }

            enum TOKEN {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            };

            StringReader json;

			SinoMoGoParser(string jsonString) {
                json = new StringReader(jsonString);
            }

            public static object Parse(string jsonString) {
				using (var instance = new SinoMoGoParser(jsonString)) {
					return instance.SinoMoGoParseValue();
                }
            }

            public void Dispose() {
                json.Dispose();
                json = null;
            }

            Dictionary<string, object> SinoMoGoParseObject() {
                Dictionary<string, object> table = new Dictionary<string, object>();

                // ditch opening brace
                json.Read();

                // {
                while (true) {
					switch (SinoMoGoNextToken) {
                    case TOKEN.NONE:
                        return null;
                    case TOKEN.COMMA:
                        continue;
                    case TOKEN.CURLY_CLOSE:
                        return table;
                    default:
                        // name
						string name = SinoMoGoParseString();
                        if (name == null) {
                            return null;
                        }

                        // :
						if (SinoMoGoNextToken != TOKEN.COLON) {
                            return null;
                        }
                        // ditch the colon
                        json.Read();

                        // value
						table[name] = SinoMoGoParseValue();
                        break;
                    }
                }
            }

			List<object> SinoMoGoParseArray() {
                List<object> array = new List<object>();

                // ditch opening bracket
                json.Read();

                // [
                var parsing = true;
                while (parsing) {
					TOKEN nextToken = SinoMoGoNextToken;

                    switch (nextToken) {
                    case TOKEN.NONE:
                        return null;
                    case TOKEN.COMMA:
                        continue;
                    case TOKEN.SQUARED_CLOSE:
                        parsing = false;
                        break;
                    default:
						object value = SinoMoGoParseByToken(nextToken);

                        array.Add(value);
                        break;
                    }
                }

                return array;
            }

            object SinoMoGoParseValue() {
				TOKEN nextToken = SinoMoGoNextToken;
				return SinoMoGoParseByToken(nextToken);
            }

			object SinoMoGoParseByToken(TOKEN token) {
                switch (token) {
                case TOKEN.STRING:
					return SinoMoGoParseString();
                case TOKEN.NUMBER:
					return SinoMoGoParseNumber();
                case TOKEN.CURLY_OPEN:
					return SinoMoGoParseObject();
                case TOKEN.SQUARED_OPEN:
					return SinoMoGoParseArray();
                case TOKEN.TRUE:
                    return true;
                case TOKEN.FALSE:
                    return false;
                case TOKEN.NULL:
                    return null;
                default:
                    return null;
                }
            }

            string SinoMoGoParseString() {
                StringBuilder s = new StringBuilder();
                char c;

                // ditch opening quote
                json.Read();

                bool parsing = true;
                while (parsing) {

                    if (json.Peek() == -1) {
                        parsing = false;
                        break;
                    }

					c = SinoMoGoNextChar;
                    switch (c) {
                    case '"':
                        parsing = false;
                        break;
                    case '\\':
                        if (json.Peek() == -1) {
                            parsing = false;
                            break;
                        }

						c = SinoMoGoNextChar;
                        switch (c) {
                        case '"':
                        case '\\':
                        case '/':
                            s.Append(c);
                            break;
                        case 'b':
                            s.Append('\b');
                            break;
                        case 'f':
                            s.Append('\f');
                            break;
                        case 'n':
                            s.Append('\n');
                            break;
                        case 'r':
                            s.Append('\r');
                            break;
                        case 't':
                            s.Append('\t');
                            break;
                        case 'u':
                            var hex = new char[4];

                            for (int i=0; i< 4; i++) {
								hex[i] = SinoMoGoNextChar;
                            }

                            s.Append((char) Convert.ToInt32(new string(hex), 16));
                            break;
                        }
                        break;
                    default:
                        s.Append(c);
                        break;
                    }
                }

                return s.ToString();
            }

			object SinoMoGoParseNumber() {
				string number = SinoMoGoNextWord;

                if (number.IndexOf('.') == -1) {
                    long parsedInt;
                    Int64.TryParse(number, out parsedInt);
                    return parsedInt;
                }

                double parsedDouble;
                Double.TryParse(number, out parsedDouble);
                return parsedDouble;
            }

			void SinoMoGoEatWhitespace() {
				while (Char.IsWhiteSpace(SinoMoGoPeekChar)) {
                    json.Read();

                    if (json.Peek() == -1) {
                        break;
                    }
                }
            }

			char SinoMoGoPeekChar {
                get {
                    return Convert.ToChar(json.Peek());
                }
            }

			char SinoMoGoNextChar {
                get {
                    return Convert.ToChar(json.Read());
                }
            }

			string SinoMoGoNextWord {
                get {
                    StringBuilder word = new StringBuilder();

					while (!IsWordBreak(SinoMoGoPeekChar)) {
						word.Append(SinoMoGoNextChar);

                        if (json.Peek() == -1) {
                            break;
                        }
                    }

                    return word.ToString();
                }
            }

			TOKEN SinoMoGoNextToken {
                get {
					SinoMoGoEatWhitespace();

                    if (json.Peek() == -1) {
                        return TOKEN.NONE;
                    }

					switch (SinoMoGoPeekChar) {
                    case '{':
                        return TOKEN.CURLY_OPEN;
                    case '}':
                        json.Read();
                        return TOKEN.CURLY_CLOSE;
                    case '[':
                        return TOKEN.SQUARED_OPEN;
                    case ']':
                        json.Read();
                        return TOKEN.SQUARED_CLOSE;
                    case ',':
                        json.Read();
                        return TOKEN.COMMA;
                    case '"':
                        return TOKEN.STRING;
                    case ':':
                        return TOKEN.COLON;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return TOKEN.NUMBER;
                    }

					switch (SinoMoGoNextWord) {
                    case "false":
                        return TOKEN.FALSE;
                    case "true":
                        return TOKEN.TRUE;
                    case "null":
                        return TOKEN.NULL;
                    }

                    return TOKEN.NONE;
                }
            }
        }

        /// <summary>
        /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
        /// </summary>
        /// <param name="json">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        /*public static string Serialize(object obj) {
            return Serializer.Serialize(obj);
        }

        sealed class Serializer {
            StringBuilder builder;

            Serializer() {
                builder = new StringBuilder();
            }

            public static string Serialize(object obj) {
                var instance = new Serializer();

                instance.SerializeValue(obj);

                return instance.builder.ToString();
            }

            void SerializeValue(object value) {
                IList asList;
                IDictionary asDict;
                string asStr;

                if (value == null) {
                    builder.Append("null");
                } else if ((asStr = value as string) != null) {
                    SerializeString(asStr);
                } else if (value is bool) {
                    builder.Append((bool) value ? "true" : "false");
                } else if ((asList = value as IList) != null) {
                    SerializeArray(asList);
                } else if ((asDict = value as IDictionary) != null) {
                    SerializeObject(asDict);
                } else if (value is char) {
                    SerializeString(new string((char) value, 1));
                } else {
                    SerializeOther(value);
                }
            }

            void SerializeObject(IDictionary obj) {
                bool first = true;

                builder.Append('{');

                foreach (object e in obj.Keys) {
                    if (!first) {
                        builder.Append(',');
                    }

                    SerializeString(e.ToString());
                    builder.Append(':');

                    SerializeValue(obj[e]);

                    first = false;
                }

                builder.Append('}');
            }

            void SerializeArray(IList anArray) {
                builder.Append('[');

                bool first = true;

                foreach (object obj in anArray) {
                    if (!first) {
                        builder.Append(',');
                    }

                    SerializeValue(obj);

                    first = false;
                }

                builder.Append(']');
            }

            void SerializeString(string str) {
                builder.Append('\"');

                char[] charArray = str.ToCharArray();
                foreach (var c in charArray) {
                    switch (c) {
                    case '"':
                        builder.Append("\\\"");
                        break;
                    case '\\':
                        builder.Append("\\\\");
                        break;
                    case '\b':
                        builder.Append("\\b");
                        break;
                    case '\f':
                        builder.Append("\\f");
                        break;
                    case '\n':
                        builder.Append("\\n");
                        break;
                    case '\r':
                        builder.Append("\\r");
                        break;
                    case '\t':
                        builder.Append("\\t");
                        break;
                    default:
                        int codepoint = Convert.ToInt32(c);
                        if ((codepoint >= 32) && (codepoint <= 126)) {
                            builder.Append(c);
                        } else {
                            builder.Append("\\u");
                            builder.Append(codepoint.ToString("x4"));
                        }
                        break;
                    }
                }

                builder.Append('\"');
            }

            void SerializeOther(object value) {
                // NOTE: decimals lose precision during serialization.
                // They always have, I'm just letting you know.
                // Previously floats and doubles lost precision too.
                if (value is float) {
                    builder.Append(((float) value).ToString("R"));
                } else if (value is int
                    || value is uint
                    || value is long
                    || value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is ulong) {
                    builder.Append(value);
                } else if (value is double
                    || value is decimal) {
                    builder.Append(Convert.ToDouble(value).ToString("R"));
                } else {
                    SerializeString(value.ToString());
                }
            }
        }*/
    }