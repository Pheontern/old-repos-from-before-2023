using System;
using System.Threading;
using static Main.ConsoleUtils;

namespace Main
{
    public static class Snake
    {

        static Random rand = new Random();

        //Hanterar input-tangenter.
        static ConsoleKey keyStorage;
        static ConsoleKey key;

        //Ormens längd och hastighet.
        static int length;
        static int speed;

        //Hanterar nuvarande rörelseriktning.
        static string directionStr;
        static char direction;

        //Arrayen för hela brädet/spelplanen.
        static char[,] board = new char[18, 36];

        //Storleken på brädet.
        static int[] linesAndChars = { board.GetLength(0), board.GetLength(1) };
        
        //Nuvarande position, samt position för svansens ände. När ormen rör sig sätts änden tillbaka till blankt från hashtag.
        static int posY, posX, tempY, tempX;

        //Brädets position. Kan ändras om man vill ha text ovanför till exempel. Men resten av programmet är inte riktigt anpassat för att kunna ändra detta.
        static int[] boardPosition = { 0, 0 };

        //Initierar alla variabler och skapar brädet så att spelet kan starta. Kör endast en gång.
        public static void Initialize()
        {

            Console.CursorVisible = false;

            Console.Clear();

            //Initierar alla använda variabler. Jag gör det här istället för vid deklarationen för att värdena ska återställas vid dödsfall.
            keyStorage = ConsoleKey.P;
            key = ConsoleKey.M;
            length = 3;
            speed = 400;
            directionStr = " ";
            direction = ' ';
            posY = linesAndChars[0] / 2;
            posX = linesAndChars[1] / 2;
            tempY = posY;
            tempX = posX;

            CreateBoard();

            Food(false);

        }

        //Fyller i den tvådimensionella arrayen med hela brädet i.
        public static void CreateBoard()
        {

            //Sätter alla värden för höger- och vänster-kanten
            for (int i = 0; i < linesAndChars[0] - 1; i++)
            {

                board[i, 0] = '|';
                board[i, linesAndChars[1] - 1] = '|';

            }

            //Sätter alla värden för nedre och övre kanten.
            for (int i = 0; i < linesAndChars[1]; i++)
            {

                board[0, i] = '-';
                board[linesAndChars[0] - 1, i] = '-';

            }

            //Fyller spelytan med tomma rutor.
            for (int i = 1; i < linesAndChars[0] - 1; i++)
            {

                for (int j = 1; j < linesAndChars[1] - 1; j++) board[i, j] = ' ';

            }

        }

        //Kör om och om igen tills spelaren avbryter. Uppdaterar brädet.
        public static void Update()
        {

            //Kallar på alla metoder i rätt ordning samt väntar mellan varje repetition (med en hastighet som minskar vid varje förlängning).

            NoFlickerClear(linesAndChars, boardPosition);

            Movement();

            Food(true);

            Losing();

            InfoText();

            Draw();

            Thread.Sleep(speed / 4);

        }

        //Ritar brädet.
        public static void Draw()
        {

            //Skapar en enda lång string med hela brädet.
            string boardStr = "";
            for (int i = 0; i < linesAndChars[0]; i++)
            {

                for (int j = 0; j < linesAndChars[1]; j++) boardStr += board[i, j];

                boardStr += "\n";

            }

            //Och skriver ut.
            Console.Write(boardStr);

        }

        //Genererar slumpmässig mat och kollar om den äts.
        public static void Food(bool checker)
        {
            
            //Om checker är true ska den kontrollera om ormen precis ätit något samt genereera en ny matbit.
            if (checker == true)
            {

                //Om ormens huvud nått en matbit...
                if (board[posY, posX] == 'O')
                {

                    int y, x;

                    //...så genereras en ny.
                    while (true)
                    {
                        y = rand.Next(1, linesAndChars[0] - 1);
                        x = rand.Next(1, linesAndChars[1] - 1);

                        if (board[y, x] == ' ') break;
                    }

                    //Den nya placeras ut.
                    board[y, x] = 'O';

                    //Ormens längd ökar med ett.
                    length++;

                    //Hastigheten ökar med ett (eftersom speed är väntetiden minskar variabeln).
                    speed--;

                }
            }
            
            //Annars genererar och placerar den bara ut en ny matbit.
            else if (checker == false)
            {

                int y, x;

                while (true)
                {
                    y = rand.Next(1, linesAndChars[0] - 1);
                    x = rand.Next(1, linesAndChars[1] - 1);

                    if (board[y, x] == ' ') break;
                }

                board[y, x] = 'O';

            }

        }

        //Ormens rörelse.
        public static void Movement()
        {

            //Om en tangent tryckts ner sparas den i key-variabeln.
            if (Console.KeyAvailable) key = Console.ReadKey(true).Key;
            
            //Om key inte är lika med den senaste knapptryckningen (alltså så att riktningen skulle förbli densamma) sparas korrekt ny riktning ner i direction.
            if (key != keyStorage)
            {

                switch (key)
                {

                    case ConsoleKey.UpArrow:
                    case ConsoleKey.W:
                        if (direction != 'S') direction = 'N';
                        break;

                    case ConsoleKey.RightArrow:
                    case ConsoleKey.D:
                        if (direction != 'W') direction = 'E';
                        break;

                    case ConsoleKey.DownArrow:
                    case ConsoleKey.S:
                        if (direction != 'N') direction = 'S';
                        break;

                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.A:
                        if (direction != 'E') direction = 'W';
                        break;

                    //Kontrollerar om användaren vill avsluta spelet med X.
                    case ConsoleKey.X:
                        Console.Clear();
                        Environment.Exit(0);
                        break;

                }

                //Sparar nuvarande tangent för att jämföra med till nästa gång.
                keyStorage = key;

            }
            //Annnars rensas sparade inputs. Om jag inte gjorde detta skulle man kunna hålla inne en tangent vilket skulle leda till att ormens riktning fastnar.
            else ClearInputBuffer();

            //Huvudets position ändras utifrån korrekt vald riktning.
            switch (direction)
            {

                case 'N':
                    posY -= 1;
                    break;

                case 'E':
                    posX += 1;
                    break;

                case 'S':
                    posY += 1;
                    break;

                case 'W':
                    posX -= 1;
                    break;

            }

            //Så länge riktningen har ett värde läggs den på i den sparade listan på senaste riktningar. Den används för att svansen ska kunna följa i huvudets spår.
            if (direction != ' ') directionStr += direction;

            //Så länge listan med riktningar är lika lång som ormen flyttar den tempY och tempX för att rensa längst bak på ormen.
            if (directionStr.Length == length)
            {
                
                board[tempY, tempX] = ' ';

                switch (directionStr[0])
                {

                    case 'N':
                        tempY -= 1;
                        break;

                    case 'E':
                        tempX += 1;
                        break;

                    case 'S':
                        tempY += 1;
                        break;

                    case 'W':
                        tempX -= 1;
                        break;

                }

                directionStr = directionStr.Remove(0, 1);

            }

        }

        //Kontrollerar om något som skulle leda till förlust hänt.
        public static void Losing()
        {

            if (posY <= 0 || posY >= linesAndChars[0] - 1 || posX <= 0 || posX >= linesAndChars[1] - 1 || (board[posY, posX] == '#' && directionStr != " ")) GameOver();
            else board[posY, posX] = '#';

        }

        //Informationstext under brädet.
        public static void InfoText()
        {

            //Sparar text som ska skrivas.
            string text = $"Nuvarande längd: {length}\nHastighet: {101 - (speed / 4)} \n\nPiltangenterna eller WASD!\nX för att avsluta.";

            //Beräknar arean för texten och sparar korrekt position där den ska skrivas ut.
            int[] area = LinesAndChars(text);
            int[] origin = { boardPosition[0], linesAndChars[0] + 1 };

            //Rensar ytan.
            NoFlickerClear(area, origin);

            //Och skriver ut på nytt. Eftersom texten måste uppdateras hela tiden.
            Console.Write(text);
            Console.SetCursorPosition(boardPosition[0], boardPosition[1]);

        }

        //Game over.
        public static void GameOver()
        {

            Games.SnakeGame();

        }

    }

}
