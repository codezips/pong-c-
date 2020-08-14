# Author: Raza Tahir
# Email: Razat51@csu.fullerton.edu
# Course: CPSC 223n ( C# )
# Final Assignment.
# Due Date: December 18th, 2016 at 2 am
# Purpose: Pong Game
# Purpose of this script: Used for compiling, linking, and running the program.

# HOW TO EXECUTE THIS PROGRAM
# In the terminal window, navigate to where this folder resides.
# Once you are in the directory ".../C#_Pong_Game",
# Enter the command "./build.sh"
# and then you should be good to go.
# NOTE: This file, build.sh, must have executable permission. 
# If it doesn't, then you won't be able to execute the command.

# There have been a few times where I have not been able to get
# the program to open correctly. If you encounter any cases like these,
# please close the window, and try running the shell script again.

# CONTROLS OF THE GAME.

# The paddle on the left side of the screen belongs to the the "Left Player".
# The left player can only move up or down, by pressing the keys
# "w" or "s" respectively.

# The paddle on the right side of the screen belongs to the the "Right Player".
# The right player can only move up or down, by pressing the keys
# "o" or "l" respectively.

# RULES
# The rules are simple enough. Each player controls one of two paddles, 
# and their job is to hit the moving ball into the other players goal
# post, while at the same time guarding their own. This game only allows 5
# tries, so whoever gets the most points out of 5 wins. 
# I believe that just about covers it. If you have any further questions, please do not
# hesitate to contact me. Hope you have fun with this game!

echo Remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile logic.cs to create the file: logic.dll
mcs -target:library logic.cs -r:System.Windows.Forms.dll -out:logic.dll

echo Compile framework.cs to create the file: framework.dll
mcs -target:library framework.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:logic.dll -out:framework.dll

echo Compile main.cs and link previously created dll file to create an executable file 
mcs main.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:framework.dll -out:Protoman.exe

echo View the list of files in the current folder
ls -l

echo Run the Moving Paddle program.
./Protoman.exe

echo The script has terminated.
