using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;
bool fastSpeed = false;
int fastMove = 3;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

InitializeGame();
while (!shouldExit)
{
    Move(true);
    if (TerminalResized())
        TerminateGame("Console was resized. ");
    foodEaten();
}

bool isFast()
{
    if (player == "(^-^)")
        return true;
    else
        return false;
}

bool isSick()
{
    if (player == "(X_X)")
        return true;
    else
        return false;
}

void foodEaten()
{
    if (playerX == foodX && playerY == foodY)
    {
        ChangePlayer();
        ShowFood();
    }

    if (isSick())
        FreezePlayer();

    fastSpeed = isFast();
}

void TerminateGame(string message = "")
{
    Console.Clear();
    Console.WriteLine($"{message}Program exiting.");
    shouldExit = true;
}

// Returns true if the Terminal was resized 
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random food at a random location
void ShowFood()
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Changes the player to match the food consumed
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Temporarily stops the player from moving
void FreezePlayer()
{
    System.Threading.Thread.Sleep(2000);
    player = states[0];
}

// Reads directional input from the Console and moves the player
void Move(bool strictKeys = false)
{
    int lastX = playerX;
    int lastY = playerY;
    bool wasStrict = false;
    int move = (fastSpeed ? fastMove : 1);

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            wasStrict = true;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            wasStrict = true;
            break;
        case ConsoleKey.LeftArrow:
            playerX -= move;
            wasStrict = true;
            break;
        case ConsoleKey.RightArrow:
            playerX += move;
            wasStrict = true;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
    }

    // If Strict Keys, then if anything other than move buttons are used, exit
    if (strictKeys && !wasStrict)
    {
        TerminateGame("Non directional key was pressed. ");
        return;
    }

    // Clear the characters at the previous position
        Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Clears the console, displays the food and player
void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}