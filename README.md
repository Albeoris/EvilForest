# Evil Forest 
    Final Fantasy IX Event Engine Tools

## Project Overview

This project is a toolkit for editing `.eb` files from the game Final Fantasy IX, which contain bytecode that describes events in battles, on game locations, and the global map.

**Final Goal**: The ultimate goal of this project is to decompile all events into understandable C# code, operating with abstractions sufficient to replay the events in the game without altering the original logic.

**Immediate Goals**: Our immediate aims are to achieve maximally readable code for all events and to learn how to convert it back into bytecode. This won't allow for writing arbitrary C# code, as you are still constrained by the capabilities of the bytecode. However, it will make editing game scripts much more convenient.

**Core Concept**: A primary idea of the project is the optimization and reorganization of the code in a way that facilitates the writing of new game events. Therefore, the decompiled code may significantly differ from a line-by-line interpretation of the bytecode, but it must functionally perform the same actions.

## Testing and Contibutions

You can use [this](https://github.com/Albeoris/EvilForest/releases/tag/v2024.05.01) archive to test the program. Feel free to make PR if you want to help us.
