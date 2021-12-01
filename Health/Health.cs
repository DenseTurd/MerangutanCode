public class Health
{
    public int hp;
    public int maxHp;

    public Health(int setMaxHp)
    {
        maxHp = setMaxHp;
    }

    public bool TakeDamage()
    {
        hp -= 1;
        return Ded();
    }

    public bool Ded()
    {
        return hp < 0 ? true : false;
    }

    public void Heal(int healing)
    {
        if (hp < maxHp)
        {
            hp += healing;
        }
    }
}
