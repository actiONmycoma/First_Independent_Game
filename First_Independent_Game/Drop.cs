using First_Independent_Game.Models;
using SFML.Learning;
using System;

namespace First_Independent_Game
{
    internal class Drop : Game
    {
        private static Random rnd = new Random();

        public float x = rnd.Next(25, 1000);
        public float y = -100;
        public float speed;
        public string sprite;

        public int id;

        public Collider collider = new Collider();
        

        public void Move()
        {
            y += speed * DeltaTime;
        }
        public void GetCollider()
        {
            collider.x = x + 5;
            collider.y = y;
            collider.width = 40;
            collider.height = 45;
        }
    }
}
