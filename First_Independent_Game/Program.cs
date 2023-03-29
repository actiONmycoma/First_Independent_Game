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

        private static string[] moveButtonsImage =
        {
            LoadTexture("a_key_white.png"),
            LoadTexture("d_key_white.png"),
            LoadTexture("s_key_white.png")
        };

        private static string[] buttons =
        {
            LoadTexture("yes_button.png"),
            LoadTexture("no_button.png"),
            LoadTexture("continue_button.png")
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

        private static string backgroundMusic = LoadMusic("slow_8_Bit_Retro_Funk_background.wav");

        private static string backgroundImage = LoadTexture("foodtrack.png");
        private static string blurImage = LoadTexture("blur.png");

        private static string healthBar = LoadTexture("health_bar.png");

        private static string help = LoadTexture("help.png");
        private static string kill = LoadTexture("kill.png");

        private static string gameOverImage = LoadTexture("game_over_white.png");

        private static Dog dog = new Dog()
        {
            x = 395,
            y = 560,
            direction = 0,
            speed = 500,
            sprite = dogRightMove[4]
        };


        static void Main(string[] args)
        {
            InitWindow(1024, 690, "Catch Food!");

            SetFont("Webcomic.ttf");

            int bestScore = 0;
            bool isExit = false;

            while (!isExit)
            {
                PlayMusic(backgroundMusic, 7);

                List<Drop> drops = new List<Drop>();

                int timeCount = 0;
                int spawnMultiplier = 0;
                int hp = 503;
                int score = 0;

                bool killButtonDown = false;
                bool helpButtonDown = false;

                bool isKilled = false;

                while (true)
                {
                    DispatchEvents();

                    if (killButtonDown)
                    {
                        bool isYesPressed = GetMouseButton(0) && MouseX >= 200
                            && MouseY >= 500 && MouseX <= 417 && MouseY <= 608;

                        bool isNoPressed = GetMouseButton(0) && MouseX >= 600
                            && MouseY >= 500 && MouseX <= 817 && MouseY <= 608;

                        if (isNoPressed) killButtonDown = false;
                        if (isYesPressed)
                        {
                            PlaySound(dogSounds[(int)DogSounds.Ouch]);
                            killButtonDown = false;
                            isKilled = true;
                        }
                    }
                    if (helpButtonDown)
                    {
                        bool isContinuePressed = GetMouseButton(0) && MouseX >= 350
                            && MouseY >= 580 && MouseX <= 638 && MouseY <= 680;

                        if (isContinuePressed) helpButtonDown = false;
                    }

                    if (isKilled)
                    {
                        bool isYesPressed = GetMouseButton(0) && MouseX >= 200
                            && MouseY >= 560 && MouseX <= 417 && MouseY <= 668;

                        bool isNoPressed = GetMouseButton(0) && MouseX >= 600
                            && MouseY >= 560 && MouseX <= 817 && MouseY <= 668;

                        if (isYesPressed) break;
                        if (isNoPressed)
                        {
                            isExit = true;
                            break;
                        }
                    }

                    if (GetMouseButton(0) == true)
                    {
                        if (MouseX >= 950 && MouseX <= 1010 && MouseY >= 20 && MouseY <= 85) killButtonDown = true;
                        if (MouseX >= 950 && MouseX <= 1010 && MouseY >= 100 && MouseY <= 160) helpButtonDown = true;
                    }

                    if (!killButtonDown && !helpButtonDown && !isKilled)
                    {
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
                                speed = 400 + score,
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
                                if (drops[i].id < Enum.GetValues(typeof(Food)).Length) score--;
                            }
                        }

                        for (int i = 0; i < drops.Count; i++)
                        {
                            if (dog.IsEat(drops[i]))
                            {
                                hp += GetHealOrDamage(drops[i].id);

                                score += GetScorePoints(drops[i].id);

                                PlayDogPickSound(drops[i].id);

                                drops.RemoveAt(i);
                            }
                        }

                        if (hp > 503) hp = 503;
                        if (score < 0) score = 0;

                        if (hp <= 0)
                        {
                            PlaySound(dogSounds[(int)DogSounds.Ouch]);

                            hp = 0;
                            isKilled = true;
                        }

                        if (isKilled)
                        {
                            if (score > bestScore) bestScore = score;
                        }

                    }

                    ClearWindow();

                    DrawSprite(backgroundImage, 0, 0);


                    for (int i = 0; i < drops.Count; i++)
                    {
                        DrawDrop(drops[i]);
                    }

                    DrawDog();

                    DrawSprite(healthBar, 10, 20);
                    SetFillColor(240, 39, 39);
                    FillRectangle(81, 36, hp, 30);

                    SetFillColor(0, 0, 0);
                    DrawText(70, 80, $"SCORE: {score}", 30);

                    DrawSprite(kill, 950, 20);
                    DrawSprite(help, 950, 100);                   

                    if (killButtonDown || helpButtonDown || isKilled)
                    {
                        DrawSprite(blurImage, 0, 0);

                        if (killButtonDown) DrawSuicideMenu();
                        if (helpButtonDown) DrawHelpMenu();
                        if (isKilled) DrawEndGameMenu(score, bestScore);
                    }

                    DisplayWindow();

                    Delay(1);
                }

            }

        }

        private static void DrawDog()
        {
            if (dog.direction == -1) DrawSprite(dogLeftMove[4], dog.x, dog.y);
            if (dog.direction == 0) DrawSprite(dogRightMove[4], dog.x, dog.y);

            if (dog.direction == 1) DrawSprite(dogLeftMove[0], dog.x, dog.y);

            if (dog.direction == 2) DrawSprite(dogRightMove[0], dog.x, dog.y);
        }

        private static void DrawDrop(Drop drop)
        {
            DrawSprite(drop.sprite, drop.x, drop.y);
        }

        private static void DrawHelpMenu()
        {
            SetFillColor(255, 255, 255);

            DrawSprite(moveButtonsImage[0], 70, 120);
            DrawSprite(moveButtonsImage[1], 67, 220);
            DrawSprite(moveButtonsImage[2], 66, 315);

            DrawText(167, 137, "move left", 40);
            DrawText(167, 237, "move right", 40);
            DrawText(167, 337, "if you want to seat", 40);

            for (int i = 0; i < foodImage.Length; i++)
            {
                DrawSprite(foodImage[i], 70 + i * 60, 450);
            }

            DrawText(260, 460, "eat this to heal and take points", 30);

            for (int i = 0; i < dangerImage.Length; i++)
            {
                DrawSprite(dangerImage[i], 70 + i * 70, 520);
            }

            DrawText(260, 520, "avoid this to stay alive", 30);

            DrawSprite(buttons[2], 350, 580);

        }

        private static void DrawSuicideMenu()
        {
            SetFillColor(210, 10, 46);

            DrawText(220, 300, "WANNA  SUICIDE?", 80);

            DrawSprite(buttons[0], 200, 500);
            DrawSprite(buttons[1], 600, 500);
        }

        private static void DrawEndGameMenu(int score, int bestScore)
        {
            SetFillColor(255, 255, 255);

            DrawSprite(gameOverImage, 330, 100);

            DrawText(200, 400, $"score: {score}", 30);
            DrawText(600, 400, $"best score: {bestScore}", 30);

            DrawText(340, 500, "go to main menu?", 40);

            DrawSprite(buttons[0], 200, 560);
            DrawSprite(buttons[1], 600, 560);
        }

        private static void PlayDogPickSound(int dropId)
        {
            if (dropId == (int)Food.Pizza) PlaySound(dogSounds[(int)DogSounds.Eat]);
            if (dropId == (int)Food.Choco) PlaySound(dogSounds[(int)DogSounds.Slurp]);
            if (dropId == (int)Food.Burger) PlaySound(dogSounds[(int)DogSounds.Bark2]);
            if (dropId == (int)Danger.Knife) PlaySound(dogSounds[(int)DogSounds.Whine]);
            if (dropId == (int)Danger.Rock) PlaySound(dogSounds[(int)DogSounds.Smack]);
        }

        private static (string sprite, int id) GetRandomDropItem(int score)
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
                id = index + Enum.GetValues(typeof(Food)).Length;
            }
            else
            {
                int index = rnd.Next(foodImage.Length);
                image = foodImage[index];
                id = index;
            }

            return (image, id);
        }

        private static int GetHealOrDamage(int dropId)
        {
            if (dropId == (int)Food.Pizza) return 5;
            if (dropId == (int)Food.Choco) return 10;
            if (dropId == (int)Food.Burger) return 15;

            if (dropId == (int)Danger.Knife) return -50;
            if (dropId == (int)Danger.Rock) return -100;

            return 0;
        }

        private static int GetScorePoints(int dropId)
        {
            if (dropId == (int)Food.Pizza) return 1;
            if (dropId == (int)Food.Choco) return 2;
            if (dropId == (int)Food.Burger) return 3;

            if (dropId == (int)Danger.Knife) return -1;
            if (dropId == (int)Danger.Rock) return -2;

            return 0;
        }
    }
}
