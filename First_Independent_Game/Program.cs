using System;
using System.IO;
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
            Rock = 3,
            Knife
        }
        private enum Buttons
        {
            Yes,
            No,
            Continue,
            Play,
            Kill,
            Help,
            Quit,
            PlayMusic,
            UnplayMusic,
            Volume,
            NoVolume,
            Back
        }

        private static string[] dogRightMoveSprites =
        {
            LoadTexture("move_right_0.png"),
            LoadTexture("move_right_1.png"),
            LoadTexture("move_right_2.png"),
            LoadTexture("move_right_3.png"),

            LoadTexture("no_move_sit_right.png")
        };
        private static string[] dogLeftMoveSprites =
        {
            LoadTexture("move_left_0.png"),
            LoadTexture("move_left_1.png"),
            LoadTexture("move_left_2.png"),
            LoadTexture("move_left_3.png"),

            LoadTexture("no_move_sit_left.png")
        };

        private static string[] foodSprites =
        {
            LoadTexture("pizza.png"),
            LoadTexture("choco.png"),
            LoadTexture("burger.png")
        };

        private static string[] dangerSprites =
        {
            LoadTexture("rock.png"),
            LoadTexture("knife.png")
        };

        private static string[] moveButtonsSprites =
        {
            LoadTexture("a_key_white.png"),
            LoadTexture("d_key_white.png"),
            LoadTexture("s_key_white.png")
        };

        private static string[] buttonsSprites =
        {
            LoadTexture("yes_button.png"),
            LoadTexture("no_button.png"),
            LoadTexture("continue_button.png"),
            LoadTexture("play_button.png"),
            LoadTexture("kill.png"),
            LoadTexture("help.png"),
            LoadTexture("quit_button.png"),
            LoadTexture("play_music_button.png"),
            LoadTexture("play_music_button_grey.png")

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

        private static string gameOverImage = LoadTexture("game_over_white.png");

        private static Dog dog = new Dog()
        {
            x = 395,
            y = 560,
            direction = 0,
            speed = 500,
            sprite = dogRightMoveSprites[4]
        };

        private static int bestScore = 0;

        static void Main(string[] args)
        {
            InitWindow(1024, 690, "Catch Food!");

            SetFont("Webcomic.ttf");

            //string[] bestScoreArr = GetBestScoreArr();

            bool isExit = false;

            while (!isExit)
            {
                int volume = 7;
                bool isPlayMusic = true;

                int menuTimeCount = 0;

                while (true)
                {
                    DispatchEvents();

                    if (isPlayMusic)
                        PlayMusic(backgroundMusic, volume);
                    else
                        StopMusic(backgroundMusic);

                    bool isPlayPressed = GetMouseButton(0) && MouseX >= 100
                             && MouseY >= 150 && MouseX <= 398 && MouseY <= 222;
                    bool isQuitPressed = GetMouseButton(0) && MouseX >= 100
                             && MouseY >= 550 && MouseX <= 398 && MouseY <= 622;

                    bool isMusicPressed = GetMouseButton(0) && MouseX >= 100
                             && MouseY >= 280 && MouseX <= 180 && MouseY <= 359;//80:79


                    if (isPlayPressed) break;

                    if (isMusicPressed && menuTimeCount > 20)
                    {
                        if (isPlayMusic)
                            isPlayMusic = false;
                        else
                            isPlayMusic = true;

                        menuTimeCount = 0;
                    }


                    if (isQuitPressed && menuTimeCount > 20)
                    {
                        isExit = true;
                        break;
                    }

                    ClearWindow();

                    DrawSprite(backgroundImage, 0, 0);
                    DrawDog();
                    DrawSprite(blurImage, 0, 0);

                    DrawMainMenu(isPlayMusic);

                    DisplayWindow();

                    Delay(1);

                    menuTimeCount++;
                }

                List<Drop> drops = new List<Drop>();

                int timeCount = 0;
                int spawnMultiplier = 0;
                int hp = 503;
                int score = 0;

                float oldDogX = dog.x;
                int deltaMove = 0;

                bool killButtonDown = false;
                bool helpButtonDown = false;

                bool isKilled = false;
                bool isInBest = false;

                while (!isExit)
                {
                    DispatchEvents();

                    if (isPlayMusic)
                        PlayMusic(backgroundMusic, volume);
                    else
                        StopMusic(backgroundMusic);

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
                        if (isInBest)
                        {
                            WriteInBestFile(score);
                            isInBest = false;
                        }

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


                    if (!killButtonDown && !helpButtonDown && !isKilled)
                    {
                        if (GetMouseButton(0) == true)
                        {
                            if (MouseX >= 950 && MouseX <= 1010 && MouseY >= 20 && MouseY <= 85) killButtonDown = true;
                            if (MouseX >= 950 && MouseX <= 1010 && MouseY >= 100 && MouseY <= 160) helpButtonDown = true;
                        }

                        int oldDogDirection = dog.direction;

                        dog.Move();
                        dog.GetCollider();

                        if (oldDogDirection == dog.direction && oldDogX != dog.x)
                        {
                            deltaMove++;
                            oldDogX = dog.x;
                        }
                        else
                        {
                            deltaMove = 0;
                        }

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
                            isInBest = CanIncludeInBestArr(score);
                        }

                    }

                    ClearWindow();

                    DrawSprite(backgroundImage, 0, 0);

                    for (int i = 0; i < drops.Count; i++)
                    {
                        DrawDrop(drops[i]);
                    }

                    DrawDog(deltaMove);

                    DrawSprite(healthBar, 10, 20);
                    SetFillColor(240, 39, 39);
                    FillRectangle(81, 36, hp, 30);

                    SetFillColor(0, 0, 0);
                    DrawText(70, 80, $"SCORE: {score}", 30);

                    DrawSprite(buttonsSprites[(int)Buttons.Kill], 950, 20);
                    DrawSprite(buttonsSprites[(int)Buttons.Help], 950, 100);

                    if (killButtonDown || helpButtonDown || isKilled)
                    {
                        DrawSprite(blurImage, 0, 0);

                        if (killButtonDown) DrawSuicideMenu();
                        if (helpButtonDown) DrawHelpMenu();
                        if (isKilled) DrawEndGameMenu(score);
                    }

                    DisplayWindow();

                    Delay(1);
                }

            }

        }

        private static void DrawDog(int deltaMove = 0)
        {
            if (dog.direction == -1)
            {
                DrawSprite(dogLeftMoveSprites[dogLeftMoveSprites.Length - 1], dog.x, dog.y);
                return;
            }
            if (dog.direction == 0)
            {
                DrawSprite(dogRightMoveSprites[dogRightMoveSprites.Length - 1], dog.x, dog.y);
                return;
            }

            string[] frames = _getFramesArr();
            int frameIndex = deltaMove % 12;

            DrawSprite(frames[frameIndex], dog.x, dog.y);
        }
        private static void DrawDrop(Drop drop)
        {
            DrawSprite(drop.sprite, drop.x, drop.y);
        }
        private static void DrawMainMenu(bool isPlayMusic)
        {
            DrawSprite(buttonsSprites[(int)Buttons.Play], 100, 150);
            DrawSprite(buttonsSprites[(int)Buttons.Quit], 100, 550);

            if (isPlayMusic)
            {
                DrawSprite(buttonsSprites[(int)Buttons.PlayMusic], 100, 280);
            }
            else
            {
                DrawSprite(buttonsSprites[(int)Buttons.UnplayMusic], 100, 280);
            }

            DrawText(190, 295, "background music", 40);

            SetFillColor(235, 25, 31);
            DrawText(750, 160, "Best score", 40);
            SetFillColor(255, 255, 255);

            string[] bestScoreArr = _getBestScoreArr();

            int y = 220;

            for (int i = 0; i < bestScoreArr.Length; i++)
            {
                if (bestScoreArr[i] != null)
                {
                    DrawText(750, y, bestScoreArr[i], 30);
                    y += 40;
                }
            }

        }
        private static void DrawHelpMenu()
        {
            SetFillColor(255, 255, 255);

            DrawSprite(moveButtonsSprites[0], 70, 120);
            DrawSprite(moveButtonsSprites[1], 67, 220);
            DrawSprite(moveButtonsSprites[2], 66, 315);

            DrawText(167, 137, "move left", 40);
            DrawText(167, 237, "move right", 40);
            DrawText(167, 337, "if you want to seat", 40);

            for (int i = 0; i < foodSprites.Length; i++)
            {
                DrawSprite(foodSprites[i], 70 + i * 60, 450);
            }

            DrawText(260, 460, "eat this to heal and take points", 30);

            for (int i = 0; i < dangerSprites.Length; i++)
            {
                DrawSprite(dangerSprites[i], 70 + i * 70, 520);
            }

            DrawText(260, 520, "avoid this to stay alive", 30);

            DrawSprite(buttonsSprites[(int)Buttons.Continue], 350, 580);

        }
        private static void DrawSuicideMenu()
        {
            SetFillColor(210, 10, 46);
            DrawText(220, 300, "WANNA  SUICIDE?", 80);
            SetFillColor(255, 255, 255);

            DrawSprite(buttonsSprites[(int)Buttons.Yes], 200, 500);
            DrawSprite(buttonsSprites[(int)Buttons.No], 600, 500);
        }
        private static void DrawEndGameMenu(int score)
        {
            SetFillColor(255, 255, 255);

            DrawSprite(gameOverImage, 330, 50);

            DrawText(200, 300, $"score: {score}", 30);
            DrawText(650, 300, $"best score", 30);

            string[] bestScoreArr = _getBestScoreArr();
            int y = 340;
            for (int i = 0; i < bestScoreArr.Length; i++)
            {
                if (bestScoreArr[i] != null)
                {
                    if (score == Convert.ToInt32(bestScoreArr[i])) SetFillColor(210, 10, 46);
                    DrawText(650, y, bestScoreArr[i], 25);
                    SetFillColor(255, 255, 255);
                    y += 30;
                }
            }

            DrawText(340, 500, "go to main menu?", 40);

            DrawSprite(buttonsSprites[(int)Buttons.Yes], 200, 560);
            DrawSprite(buttonsSprites[(int)Buttons.No], 600, 560);
        }
        private static void PlayDogPickSound(int dropId)
        {
            if (dropId == (int)Food.Pizza) PlaySound(dogSounds[(int)DogSounds.Eat]);
            if (dropId == (int)Food.Choco) PlaySound(dogSounds[(int)DogSounds.Slurp], 9);
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
                int index = rnd.Next(dangerSprites.Length);
                image = dangerSprites[index];
                id = index + Enum.GetValues(typeof(Food)).Length;
            }
            else
            {
                int index = rnd.Next(foodSprites.Length);
                image = foodSprites[index];
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
        private static string[] _getBestScoreArr()
        {
            string[] scoreArr = new string[5];

            StreamReader reader = new StreamReader("best_score.txt");

            string line = reader.ReadLine();

            int index = 0;
            while (line != null)
            {
                scoreArr[index++] = line;
                line = reader.ReadLine();
            }

            reader.Close();

            return scoreArr;
        }
        private static bool CanIncludeInBestArr(int score)
        {
            string[] scoreArr = _getBestScoreArr();

            for (int i = 0; i < scoreArr.Length; i++)
            {
                if (scoreArr[i] == null) return true;

                if (score > Convert.ToInt32(scoreArr[i]))
                {
                    return true;
                }
            }

            return false;
        }
        private static void WriteInBestFile(int score)
        {
            string[] bestScoreArr = _getBestScoreArr();

            int positionIndex = 0;

            int index = 0;
            while (bestScoreArr[index] != null && index < bestScoreArr.Length)
            {
                if (score > Convert.ToInt32(bestScoreArr[index]))
                {
                    positionIndex = index++;
                    break;
                }
                else if (score == Convert.ToInt32(bestScoreArr[index]))
                {
                    positionIndex = -1;
                }
                else
                {
                    positionIndex++;
                    index++;
                }
            }

            if (positionIndex != -1)
            {
                if (positionIndex > 0)
                {
                    for (int i = bestScoreArr.Length - 1; i >= positionIndex; i--)
                    {
                        bestScoreArr[i] = bestScoreArr[i - 1];
                    }
                }

                bestScoreArr[positionIndex] = Convert.ToString(score);

                StreamWriter writer = new StreamWriter("best_score.txt");

                index = 0;
                while (bestScoreArr[index] != null && index < bestScoreArr.Length)
                {
                    writer.WriteLine(bestScoreArr[index++]);
                }

                writer.Close();
            }
        }
        private static string[] _getFramesArr()
        {
            string[] frames = new string[12];

            if (dog.direction == 1)
            {
                for (int i = 0; i < dogLeftMoveSprites.Length - 1; i++)
                {
                    for (int j = i * 3; j < i * 3 + 3; j++)
                    {
                        frames[j] = dogLeftMoveSprites[i];
                    }
                }
            }
            if (dog.direction == 2)
            {
                for (int i = 0; i < dogRightMoveSprites.Length - 1; i++)
                {
                    for (int j = i * 3; j < i * 3 + 3; j++)
                    {
                        frames[j] = dogRightMoveSprites[i];
                    }
                }
            }

            return frames;
        }
    }
}
