    public interface IWeapon
    {
        string Name { get; }
        int BonusDamage { get; }
    }

    public interface IArmor
    {
        string Name { get; }
        int Defense { get; }
    }

    public interface IEnvironment
    {
        void Describe();
    }

    public class FantasyWeapon : IWeapon
    {
        public string Name => "Зачарованный меч";
        public int BonusDamage => 15;
    }

    public class FantasyArmor : IArmor
    {
        public string Name => "Магическая мантия";
        public int Defense => 10;
    }

    public class FantasyEnvironment : IEnvironment
    {
        public void Describe() => Console.WriteLine("Окружение: Древний зачарованный лес с рунами.");
    }

    public class SciFiWeapon : IWeapon
    {
        public string Name => "Плазменная винтовка";
        public int BonusDamage => 25;
    }

    public class SciFiArmor : IArmor
    {
        public string Name => "Нанотехнологичный скафандр";
        public int Defense => 20;
    }

    public class SciFiEnvironment : IEnvironment
    {
        public void Describe() => Console.WriteLine("Окружение: Высокотехнологичный неоновый мегаполис.");
    }

    public interface IGameWorldFactory
    {
        IWeapon CreateWeapon();
        IArmor CreateArmor();
        IEnvironment CreateEnvironment();
    }

    public class FantasyWorldFactory : IGameWorldFactory
    {
        public IWeapon CreateWeapon() => new FantasyWeapon();
        public IArmor CreateArmor() => new FantasyArmor();
        public IEnvironment CreateEnvironment() => new FantasyEnvironment();
    }

    public class SciFiWorldFactory : IGameWorldFactory
    {
        public IWeapon CreateWeapon() => new SciFiWeapon();
        public IArmor CreateArmor() => new SciFiArmor();
        public IEnvironment CreateEnvironment() => new SciFiEnvironment();
    }

    public abstract class CharacterClass
    {
        public string ClassName { get; protected set; }
        public int BaseHealth { get; protected set; }
        public int BaseAttack { get; protected set; }
        public string Description { get; protected set; }
    }

    public class Warrior : CharacterClass
    {
        public Warrior()
        {
            ClassName = "Воин";
            BaseHealth = 150;
            BaseAttack = 20;
            Description = "Могучий боец ближнего боя.";
        }
    }

    public class Mage : CharacterClass
    {
        public Mage()
        {
            ClassName = "Маг";
            BaseHealth = 80;
            BaseAttack = 35;
            Description = "Повелитель стихий, наносящий урон заклинаниями.";
        }
    }

    public class Archer : CharacterClass
    {
        public Archer()
        {
            ClassName = "Лучник";
            BaseHealth = 100;
            BaseAttack = 25;
            Description = "Меткий стрелок, поражающий цели на расстоянии.";
        }
    }

    public abstract class CharacterClassFactory
    {
        public abstract CharacterClass CreateCharacterClass();
    }

    public class WarriorFactory : CharacterClassFactory
    {
        public override CharacterClass CreateCharacterClass() => new Warrior();
    }

    public class MageFactory : CharacterClassFactory
    {
        public override CharacterClass CreateCharacterClass() => new Mage();
    }

    public class ArcherFactory : CharacterClassFactory
    {
        public override CharacterClass CreateCharacterClass() => new Archer();
    }

    public class GameCharacter
    {
        public string Name { get; set; }
        public CharacterClass Class { get; set; }
        public IWeapon Weapon { get; set; }
        public IArmor Armor { get; set; }
        public int Level { get; set; }
        public List<string> SpecialAbilities { get; set; } = new List<string>();

        public void DisplayInfo()
        {
            Console.WriteLine($"Персонаж: {Name}");
            Console.WriteLine($"Класс: {Class?.ClassName ?? "Не задан"} ({Class?.Description})");
            Console.WriteLine($"Уровень: {Level}");
            Console.WriteLine($"Базовое здоровье: {Class?.BaseHealth ?? 0}");
            Console.WriteLine($"Оружие: {Weapon?.Name ?? "Кулаки"} (+{Weapon?.BonusDamage ?? 0} к атаке)");
            Console.WriteLine($"Броня: {Armor?.Name ?? "Нет"} (+{Armor?.Defense ?? 0} к защите)");
            Console.WriteLine($"Способности: {(SpecialAbilities.Count > 0 ? string.Join(", ", SpecialAbilities) : "Нет")}");
            Console.WriteLine(new string('-', 30));
        }
    }

    public interface ICharacterBuilder
    {
        ICharacterBuilder SetName(string name);
        ICharacterBuilder SetClass(CharacterClass characterClass);
        ICharacterBuilder SetWeapon(IWeapon weapon);
        ICharacterBuilder SetArmor(IArmor armor);
        ICharacterBuilder SetLevel(int level);
        ICharacterBuilder AddAbility(string ability);
        GameCharacter Build();
    }

    public class CharacterBuilder : ICharacterBuilder
    {
        private GameCharacter _character;

        public CharacterBuilder() => Reset();

        public void Reset()
        {
            _character = new GameCharacter
            {
                Name = "Безымянный Герой",
                Level = 1,
                SpecialAbilities = new List<string>()
            };
        }

        public ICharacterBuilder SetName(string name) { _character.Name = name; return this; }
        public ICharacterBuilder SetClass(CharacterClass characterClass) { _character.Class = characterClass; return this; }
        public ICharacterBuilder SetWeapon(IWeapon weapon) { _character.Weapon = weapon; return this; }
        public ICharacterBuilder SetArmor(IArmor armor) { _character.Armor = armor; return this; }
        public ICharacterBuilder SetLevel(int level) { _character.Level = level; return this; }
        public ICharacterBuilder AddAbility(string ability) { _character.SpecialAbilities.Add(ability); return this; }

        public GameCharacter Build()
        {
            GameCharacter result = _character;
            Reset();
            return result;
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            CharacterBuilder builder = new CharacterBuilder();
            CharacterClassFactory warriorFactory = new WarriorFactory();
            CharacterClassFactory magesFactory = new MageFactory();

            IGameWorldFactory fantasyWorld = new FantasyWorldFactory();
            fantasyWorld.CreateEnvironment().Describe();

            GameCharacter fantasyHero = builder
                .SetName("Гэндальф")
                .SetClass(magesFactory.CreateCharacterClass())
                .SetWeapon(fantasyWorld.CreateWeapon())
                .SetArmor(fantasyWorld.CreateArmor())
                .SetLevel(42)
                .AddAbility("Телепортация")
                .Build();

            fantasyHero.DisplayInfo();

            IGameWorldFactory sciFiWorld = new SciFiWorldFactory();
            sciFiWorld.CreateEnvironment().Describe();

            GameCharacter sciFiHero = builder
                .SetName("DoomGuy")
                .SetClass(warriorFactory.CreateCharacterClass())
                .SetWeapon(sciFiWorld.CreateWeapon())
                .SetArmor(sciFiWorld.CreateArmor())
                .AddAbility("Реактивный ранец")
                .Build();

            sciFiHero.DisplayInfo();

            Console.ReadLine();
        }
    }
