# <a id="title"></a>SINOMOGO SDK FILE CHECKER
## Unity for Android


The SinoMoGo SDK File Checker is a tool which helps to verify that all required files are present and in the correct location after the SinoMoGo SDK integration has been completed as well as verify the AndroidManifest.xml configuration. 

####Windows 
Where your development environment is Windows, use the "**SinoMoGoCheckFileTool.exe**" file found in the [Windows] (./windows) sub-directory (_Fig 1._).

 _Fig 1._ 

 ![Fig SinoMoGoCheckFileTool.exe ](../Documentation/imgs/checker01.png) 
 
The SinoMoGo SDK File Checker tool interface ( _Fig 2._ ):
 
 _Fig 2._ 

 ![Fig SinoMoGoCheckFileTool ](../Documentation/imgs/checker02.png) 

- Click the "**Browse**" button and select your project folder (which already has the SinoMoGo SDK integrated).
- Click the "**Start**" button to start the checking process.
- A successful pass will display the message "**Checking completed, no errors found**".
- In case there are errors reported ("**Not configured**", "**Not found**" messages) please check the log against the [FileList.txt] (../Documentation/FileList.txt) and amend any errors by copying the missing files to their correct location.

###Linux

Where your development environment is Linux, use the resources found in the [Linux] (./linux) sub-directory (_Fig 3._).

_Fig 3._

 ![Fig check.sh ](../Documentation/imgs/checker03.png) 


Open terminal and execute the following command with appropriate user permission:
   
  ```xml
  sh "**./<shellPath>/check.sh**" "**./<projectPath>**".
  ``` 

"**&lt;shellPath&gt;**" must be the location where `check.sh` resides and "**&lt;projectPath&gt;**" should be your game or app's project path.

- A successful pass will display the "**Configuration OK!**" and "**File OK!**" messages.
- In case there are errors reported ("**Not configured**", "**Not found**" messages) please check the log against the [FileList.txt] (../Documentation/FileList.txt) and amend any errors by copying the missing files to their correct location.
