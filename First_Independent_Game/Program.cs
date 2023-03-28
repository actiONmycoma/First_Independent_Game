using System;
using SFML.Window;
using SFML.Learning;
using System.Collections.Generic;
using First_Independent_Game;
using First_Independent_Game.Models;

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

        private static Dog dog = new Dog()
        {
            x = 395,
            y = 560,
            direction = 0,
            speed = 500,
            sprite = dogRightMove[4],
            collider = new Collider()
        };
        
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

                    dog.DogMove();
                    dog.GetCollider();

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
            if (dog.direction == -1) DrawSprite(dogLeftMove[4], dog.x, dog.y);
            if (dog.direction == 0) DrawSprite(dogRightMove[4], dog.x, dog.y);

            if (dog.direction == 1) DrawSprite(dogLeftMove[0], dog.x, dog.y);

            if (dog.direction == 2) DrawSprite(dogRightMove[0], dog.x, dog.y);
        }

        static void DrawDrop(float[] position, string image)
        {
            DrawSprite(image, position[0], position[1]);
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

            int chance = rnd.Next(100);

            if (chance <= 30)
                return dangerImage[rnd.Next(dangerImage.Length)];
            else
                return foodImage[rnd.Next(foodImage.Length)];

        }
    }
}
