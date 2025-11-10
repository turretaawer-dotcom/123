@echo off
echo Starting JsonToRoads script...

:: Activate the virtual environment
call .venv\Scripts\activate.bat

:: Run the Python script
python UnityJSONToUnrealScene.py

:: Keep the window open if there's an error
if errorlevel 1 (
    echo.
    echo An error occurred. Press any key to exit...
    pause > nul
)

:: Deactivate the virtual environment
deactivate 