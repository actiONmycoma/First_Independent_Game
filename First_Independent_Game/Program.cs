using System;
using SFML.Window;
using SFML.Learning;
using System.Collections.Generic;

namespace First_Independent_Game
{
    internal class Program : Game
    {
        private enum DogSounds
        {
            Bark,
            Bark2,
            Ouch,
            Slurp,
            Smack,
            Whine,
            Eat
        }

        private enum Food
        {
            Pizza,
            Choco,
            Burger
        }

        private enum Danger
        {
            Rock,
            Knife
        }

        private static string[] dogRightMove =
        {
            LoadTexture("move_right_0.png"),
            LoadTexture("move_right_1.png"),
            LoadTexture("move_right_2.png"),
            LoadTexture("move_right_3.png"),
            LoadTexture("no_move_sit_right.png")
        };
        private static string[] dogLeftMove =
        {
            LoadTexture("move_left_0.png"),
            LoadTexture("move_left_1.png"),
            LoadTexture("move_left_2.png"),
            LoadTexture("move_left_3.png"),
            LoadTexture("no_move_sit_left.png")
        };

        private static string[] foodImage =
        {
            LoadTexture("pizza.png"),
            LoadTexture("choco.png"),
            LoadTexture("burger.png")
        };

        private static string[] dangerImage =
        {
            LoadTexture("rock.png"),
            LoadTexture("knife.png")
        };

        private static float dogX = 395;
        private static float dogY = 560;
        private static float dogColliderX = dogX - 10;
        private static float dogColliderY = dogY - 10;
        private static int colliderHeight = 120;
        private static int colliderWidth = 130;

        private static int dogDirection = 0;
        private static float dogSpeed = 400;

        private static float dropSpeed = 400;

        static void Main(string[] args)
        {
            InitWindow(1024, 690, "Catch Food!");

            SetFont("Webcomic.ttf");

            string backgroundMusic = LoadMusic("slow_8_Bit_Retro_Funk_background.wav");

            string[] dogSounds =
            {
                LoadSound("bark.wav"),
                LoadSound("bark2.wav"),
                LoadSound("ouch.wav"),
                LoadSound("slurp.wav"),
                LoadSound("smack.wav"),
                LoadSound("whine.wav"),
                LoadSound("eat.wav")
            };

            string backgroundImage = LoadTexture("foodtrack.png");
            string blurImage = LoadTexture("blur.png");

            string healthBar = LoadTexture("health_bar.png");



            float[] newDropPosition;
            List<string> drops = new List<string>();
            List<float[]> dropPositions = new List<float[]>();

            int score = 0;

            bool isNewGame = true;
            bool isExit = false;

            //-----основной цикл игры-----
            while (!isExit)
            {


                PlayMusic(backgroundMusic, 7);

                int timeCount = 0;

                while (true)
                {
                    DispatchEvents();

                    DogMove();

                    if (timeCount++ % 100 == 0)
                    {
                        dropPositions.Add(GetDropStartPosition());
                        drops.Add(GetRandomDrop());
                    }

                    for (int i = 0; i < dropPositions.Count; i++)
                    {
                        DropMove(dropPositions[i]);
                    }

                    ClearWindow();

                    DrawSprite(backgroundImage, 0, 0);

                    //drop
                    for (int i = 0; i < drops.Count; i++)
                    {
                        DrawDrop(dropPositions[i], drops[i]);
                    }

                    //dog                    
                    DrawDog();

                    //health
                    DrawSprite(healthBar, 10, 20);
                    SetFillColor(240, 39, 39);
                    FillRectangle(81, 36, 503, 30);

                    //score
                    SetFillColor(0, 0, 0);
                    DrawText(70, 80, $"SCORE: {score}", 30);

                    DisplayWindow();

                    Delay(1);
                }

            }

        }

        static void DrawDog()
        {
            if (dogDirection == -1) DrawSprite(dogLeftMove[4], dogX, dogY);
            if (dogDirection == 0) DrawSprite(dogRightMove[4], dogX, dogY);

            if (dogDirection == 1) DrawSprite(dogLeftMove[0], dogX, dogY);

            if (dogDirection == 2) DrawSprite(dogRightMove[0], dogX, dogY);
        }

        static void DrawDrop(float[] position, string image)
        {
            DrawSprite(image, position[0], position[1]);
        }

        static void DogMove()
        {
            if (GetKey(Keyboard.Key.A) == true)
            {
                dogX -= dogSpeed * DeltaTime;
                dogDirection = 1;
            }

            if (GetKey(Keyboard.Key.D) == true)
            {
                dogX += dogSpeed * DeltaTime;
                dogDirection = 2;
            }

            if (GetKeyDown(Keyboard.Key.S) == true)
            {
                if (dogDirection == 1) dogDirection = -1;
                if (dogDirection == 2) dogDirection = 0;
            }

        }

        static void DropMove(float[] dropPosition)
        {
            dropPosition[1] += dropSpeed * DeltaTime;
        }

        static float[] GetDropStartPosition()
        {
            Random rnd = new Random();

            return new float[] { rnd.Next(25, 1000), -50 };
        }
        static string GetRandomDrop()
        {
            Random rnd = new Random();

            int food = rnd.Next(2);

            if (food == 0)
                return dangerImage[rnd.Next(dangerImage.Length)];
            else
                return foodImage[rnd.Next(foodImage.Length)];

        }
    }
}
