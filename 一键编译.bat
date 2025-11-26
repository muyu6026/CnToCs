@echo off
setlocal enabledelayedexpansion
chcp 65001 >nul
echo ========================================
echo 中文转C#编译器 - 一键编译脚本
echo ========================================
echo.

:: 设置工作目录
cd /d "%~dp0"

:: 显示当前目录
echo 当前工作目录: %CD%
echo.

:: 编译项目
echo 正在编译项目...
dotnet build
if %errorlevel% neq 0 (
    echo 编译失败，请检查错误信息
    pause
    exit /b 1
)
echo 项目编译成功！
echo.

:: 创建输出目录
if not exist "输出文件" mkdir "输出文件"

:: 遍历并编译所有.My文件
echo 开始遍历并编译所有.My文件...
echo.

:: 处理测试文件目录
if exist "测试文件\*.My" (
    echo 正在处理测试文件目录...
    for %%f in ("测试文件\*.My") do (
        echo 编译: %%f
        set inputFile=%%f
        set outputFile=%%~nf.cs
        dotnet run "!inputFile!" "输出文件\!outputFile!"
        if !errorlevel! equ 0 (
            echo   ✓ 编译成功: 输出文件\!outputFile!
        ) else (
            echo   ✗ 编译失败
        )
        echo.
    )
)

:: 处理input目录
if exist "..\input\*.My" (
    echo 正在处理input目录...
    for %%f in ("..\input\*.My") do (
        echo 编译: %%f
        set inputFile=%%f
        set outputFile=%%~nf.cs
        dotnet run "!inputFile!" "输出文件\!outputFile!"
        if !errorlevel! equ 0 (
            echo   ✓ 编译成功: 输出文件\!outputFile!
        ) else (
            echo   ✗ 编译失败
        )
        echo.
    )
)

:: 处理当前目录
if exist "*.My" (
    echo 正在处理当前目录...
    for %%f in (".\*.My") do (
        echo 编译: %%f
        set inputFile=%%f
        set outputFile=%%~nf.cs
        dotnet run "!inputFile!" "输出文件\!outputFile!"
        if !errorlevel! equ 0 (
            echo   ✓ 编译成功: 输出文件\!outputFile!
        ) else (
            echo   ✗ 编译失败
        )
        echo.
    )
)

:: 显示所有找到的.My文件和对应的输出文件
echo ========================================
echo 找到的所有.My文件和对应的输出文件：
echo ========================================
echo.

echo 测试文件目录：
for %%f in ("测试文件\*.My") do (
    echo   输入: %%f
    echo   输出: 输出文件\%%~nf.cs
    echo.
)

echo input目录：
for %%f in ("..\input\*.My") do (
    echo   输入: %%f
    echo   输出: 输出文件\%%~nf.cs
    echo.
)

echo 当前目录：
for %%f in (".\*.My") do (
    echo   输入: %%f
    echo   输出: 输出文件\%%~nf.cs
    echo.
)

:: 显示输出目录内容
echo ========================================
echo 编译完成！输出文件内容：
echo ========================================
if exist "输出文件\*.cs" (
    dir /b "输出文件\*.cs"
) else (
    echo 没有找到生成的C#文件
)
echo.

echo ========================================
echo 编译统计：
echo ========================================
set /a totalFiles=0
set /a successFiles=0

if exist "测试文件\*.My" (
    for %%f in ("测试文件\*.My") do (
        set /a totalFiles+=1
        if exist "输出文件\%%~nf.cs" (
            set /a successFiles+=1
        )
    )
)

if exist "..\input\*.My" (
    for %%f in ("..\input\*.My") do (
        set /a totalFiles+=1
        if exist "输出文件\%%~nf.cs" (
            set /a successFiles+=1
        )
    )
)

if exist "*.My" (
    for %%f in (".\*.My") do (
        set /a totalFiles+=1
        if exist "输出文件\%%~nf.cs" (
            set /a successFiles+=1
        )
    )
)

echo 总文件数: !totalFiles!
echo 成功编译: !successFiles!
echo 失败数量: %totalFiles%

echo.
pause