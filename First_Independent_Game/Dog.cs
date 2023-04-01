using First_Independent_Game.Models;
using SFML.Window;
using SFML.Learning;

namespace First_Independent_Game
{
    internal class Dog : Game
    {
        public float x;
        public float y;
        public float speed;
        public int direction;
        public string sprite;

        public Collider collider = new Collider();



        public void Move()
        {
            if (GetKey(Keyboard.Key.A) == true)
            {
                if (x > 0) x -= speed * DeltaTime;

                direction = 1;
            }

            if (GetKey(Keyboard.Key.D) == true)
            {
                if (x + collider.width < 1024) x += speed * DeltaTime;

                direction = 2;
            }

            if (GetKeyDown(Keyboard.Key.S) == true)
            {
                if (direction == 1) direction = -1;
                if (direction == 2) direction = 0;
            }

        }
        public void GetCollider()
        {
            if (direction == -1)
            {
                collider.x = x + 10;
                collider.width = 82;
            }

            if (direction == 0)
            {
                collider.x = x + 45;
                collider.width = 82;
            }

            if (direction == 1)
            {
                collider.x = x + 5;
                collider.width = 120;
            }

            if (direction == 2)
            {
                collider.x = x + 25;
                collider.width = 109;
            }

            collider.y = y + 25;
            collider.height = 60;
        }
        public bool IsEat(Drop drop)
        {
            return collider.x + collider.width >= drop.collider.x && collider.x <= drop.collider.x + drop.collider.width
                && collider.y + collider.height >= drop.collider.y && collider.y <= drop.collider.y + drop.collider.height;
        }
    }
}
