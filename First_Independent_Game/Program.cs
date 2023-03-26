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

        static void Main(string[] args)
        {
            InitWindow(1024, 690, "Catch Food!");

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

            string dogSitImage = LoadTexture("no_move_sit.png");
            string[] dogRightMove =
            {
                LoadTexture("move_right_0.png"),
                LoadTexture("move_right_1.png"),
                LoadTexture("move_right_2.png"),
                LoadTexture("move_right_3.png")
            };
            string[] dogLeftMove =
            {
                LoadTexture("move_left_0.png"),
                LoadTexture("move_left_1.png"),
                LoadTexture("move_left_2.png"),
                LoadTexture("move_left_3.png")
            };

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

            PlayMusic(backgroundMusic, 7);

            while (true)
            {
                DispatchEvents();

                ClearWindow();

                DrawSprite(backgroundImage, 0, 0);


                DisplayWindow();

                Delay(1);
            }

        }
    }
}
