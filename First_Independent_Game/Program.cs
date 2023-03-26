using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Learning;
using SFML.System;
using SFML.Window;

namespace First_Independent_Game
{
    internal class Program : Game
    {
        public enum DogSounds
        {
            Bark,
            Bark2,
            Ouch,
            Slurp,
            Smack,
            Whine,
            Eat
        }

        public enum Food
        {
            Pizza,
            Choco,
            Burger
        }

        public enum Danger
        {
            Rock,
            Knife
        }

        static string[] dogRightMove =
        {
            LoadTexture("move_right_0.png"),
            LoadTexture("move_right_1.png"),
            LoadTexture("move_right_2.png"),
            LoadTexture("move_right_3.png"),
            LoadTexture("no_move_sit_right.png")
        };
        static string[] dogLeftMove =
        {
            LoadTexture("move_left_0.png"),
            LoadTexture("move_left_1.png"),
            LoadTexture("move_left_2.png"),
            LoadTexture("move_left_3.png"),
            LoadTexture("no_move_sit_left.png")

        };

        static float dogX = 395;
        static float dogY = 560;
        static float dogColliderX = dogX - 10;
        static float dogColliderY = dogY - 10;
        static int colliderHeight = 120;
        static int colliderWidth = 130;

        static int dogDirection = 0;
        static float dogSpeed = 400;

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



            string[] foodImage =
            {
                LoadTexture("pizza.png"),
                LoadTexture("choco.png"),
                LoadTexture("burger.png")
            };
            string[] dangerImage =
            {
                LoadTexture("rock.png"),
                LoadTexture("knife.png")
            };

            string healthBar = LoadTexture("health_bar.png");


            //-----основной цикл игры-----

            int score = 0;

            bool isNewGame = true;
            bool isExit = false;

            while (!isExit)
            {


                PlayMusic(backgroundMusic, 7);

                while (true)
                {
                    DispatchEvents();

                    DogMove();

                    ClearWindow();

                    DrawSprite(backgroundImage, 0, 0);

                    //food
                    //danger

                    //dog                    
                    DrawDog();

                    //health
                    DrawSprite(healthBar, 10, 20);
                    SetFillColor(240, 39, 39);
                    FillRectangle(81, 36, 503, 30);

                    //score
                    SetFillColor(0, 0, 0);
                    DrawText(70, 80, "SCORE:", 30);

                    DisplayWindow();

                    Delay(1);
                }

            }

        }

        static void DrawDog()
        {
            if (dogDirection == -1) DrawSprite(dogLeftMove[4], dogX, dogY);
            if (dogDirection == 0) DrawSprite(dogRightMove[4], dogX, dogY);

            if (dogDirection == 1) DrawSprite(dogLeftMove[1], dogX, dogY);


            if (dogDirection == 2) DrawSprite(dogRightMove[1], dogX, dogY);

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
    }
}
