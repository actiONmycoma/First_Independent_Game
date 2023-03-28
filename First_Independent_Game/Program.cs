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
            Rock = 3,
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

        private static string[] dogSounds =
        {
                LoadSound("bark.wav"),
                LoadSound("bark2.wav"),
                LoadSound("ouch.wav"),
                LoadSound("slurp.wav"),
                LoadSound("smack.wav"),
                LoadSound("whine.wav"),
                LoadSound("eat.wav")
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


        static void Main(string[] args)
        {
            InitWindow(1024, 690, "Catch Food!");

            SetFont("Webcomic.ttf");

            string backgroundMusic = LoadMusic("slow_8_Bit_Retro_Funk_background.wav");


            string backgroundImage = LoadTexture("foodtrack.png");
            string blurImage = LoadTexture("blur.png");

            string healthBar = LoadTexture("health_bar.png");

            string help = LoadTexture("help.png");//60x60
            string kill = LoadTexture("kill.png");//60x65

            bool isNewGame = true;
            bool isExit = false;

            //-----основной цикл игры-----
            while (!isExit)
            {
                PlayMusic(backgroundMusic, 7);

                List<Drop> drops = new List<Drop>();

                int timeCount = 0;
                int score = 0;
                int spawnMultiplier = 0;
                int hp = 503;

                bool killButtonDown = false;
                bool helpButtonDown = false;

                while (true)
                {
                    DispatchEvents();

                    if (GetMouseButtonDown(0) == true)
                    {
                        if (MouseX >= 950 && MouseX <= 1010 && MouseY >= 20 && MouseY <= 85) killButtonDown = true;
                        if (MouseX >= 950 && MouseX <= 1010 && MouseY >= 100 && MouseY <= 160) helpButtonDown = true;
                    }


                    dog.Move();
                    dog.GetCollider();

                    spawnMultiplier = score;
                    if (spawnMultiplier >= 100) spawnMultiplier = 99;
                    if (timeCount > 3000 && spawnMultiplier < 30) spawnMultiplier = 30;
                    if (timeCount > 3000 * 3 && spawnMultiplier < 60) spawnMultiplier = 60;
                    if (timeCount > 3000 * 6 && spawnMultiplier < 90) spawnMultiplier = 90;

                    bool isSpawn = timeCount++ % (100 - spawnMultiplier / 10 * 10) == 0;

                    if (isSpawn)
                    {
                        (string sprite, int id) = GetRandomDropItem(score);

                        Drop drop = new Drop()
                        {
                            speed = 400,
                            sprite = sprite,
                            id = id
                        };

                        drops.Add(drop);
                    }

                    for (int i = 0; i < drops.Count; i++)
                    {
                        drops[i].Move();
                        drops[i].GetCollider();

                        if (drops[i].y > 690)
                        {
                            drops.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < drops.Count; i++)
                    {
                        if (dog.IsEat(drops[i]))
                        {
                            hp += GetDamage(drops[i].id);

                            if (drops[i].id < 3) score += GetDamage(drops[i].id) / 5;

                            PlayDogSound(drops[i].id);

                            drops.RemoveAt(i);
                        }
                    }

                    if (hp <= 0) break;

                    if (hp > 503) hp = 503;
                    if (score < 0) score = 0;

                    ClearWindow();

                    DrawSprite(backgroundImage, 0, 0);
                    
                    //drop
                    for (int i = 0; i < drops.Count; i++)
                    {
                        DrawDrop(drops[i]);
                    }
                    //dog                    
                    DrawDog();
                    //health
                    DrawSprite(healthBar, 10, 20);
                    SetFillColor(240, 39, 39);
                    FillRectangle(81, 36, hp, 30);
                    //score
                    SetFillColor(0, 0, 0);
                    DrawText(70, 80, $"SCORE: {score}", 30);
                    //menu
                    DrawSprite(kill, 950, 20);
                    DrawSprite(help, 950, 100);

                    DrawText(0, 0, Convert.ToString(timeCount), 10);

                    if (killButtonDown || helpButtonDown)
                    {
                        while (true)
                        {

                        }
                    }

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

        static void DrawDrop(Drop drop)
        {
            DrawSprite(drop.sprite, drop.x, drop.y);
        }

        static void PlayDogSound(int dropId)
        {
            if (dropId == (int)Food.Pizza) PlaySound(dogSounds[(int)DogSounds.Eat]) ;
            if (dropId == (int)Food.Choco) PlaySound(dogSounds[(int)DogSounds.Slurp]);
            if (dropId == (int)Food.Burger) PlaySound(dogSounds[(int)DogSounds.Bark2]);
            if (dropId == (int)Danger.Knife) PlaySound(dogSounds[(int)DogSounds.Whine]);
            if (dropId == (int)Danger.Rock) PlaySound(dogSounds[(int)DogSounds.Smack]);
        }

        static (string sprite, int id) GetRandomDropItem(int score)
        {
            Random rnd = new Random();

            int chance = rnd.Next(100);

            int scoreMultiplier = score / 3;

            if (scoreMultiplier > 40) scoreMultiplier = 40;

            string image;
            int id;

            if (chance <= 30 + scoreMultiplier)
            {
                int index = rnd.Next(dangerImage.Length);
                image = dangerImage[index];
                id = index + 3;
            }
            else
            {
                int index = rnd.Next(foodImage.Length);
                image = foodImage[index];
                id = index;
            }

            return (image, id);
        }

        static int GetDamage(int dropId)
        {
            if (dropId == (int)Food.Pizza) return 5;
            if (dropId == (int)Food.Choco) return 10;
            if (dropId == (int)Food.Burger) return 15;
            if (dropId == (int)Danger.Knife) return -50;
            if (dropId == (int)Danger.Rock) return -100;

            return 0;
        }
    }
}
