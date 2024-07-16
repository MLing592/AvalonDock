rem 将VS2022-Dark开头的png文件复制并改名为VS2022-Blue，输入VS2022-Blue
setlocal enabledelayedexpansion
set /p prefix=Enter prefix:
for /f "delims=" %%f in ('dir /b VS2022-Dark*.png') do (
    set h=%%f
    copy "%%f" "!prefix!!h:~11!"
)
pause