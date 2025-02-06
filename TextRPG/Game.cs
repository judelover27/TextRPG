namespace TextRPG
{
    class Program
    {
        static Player player = new Player();
        static Inventory inventory = new Inventory();
        static Shop shop = new Shop();


        static void Main()//코딩은 처음배워봐서 너무 어려워요 ㅠㅠ
        {
            string title = "크로노 트리거";
            char[] chars = title.ToCharArray();
            player.SetInventory(inventory);

            Print.P("ddddd")

            foreach (char c in chars)
            {
                Console.Write(c);
                Thread.Sleep(130);
            }

            shop.SetPlayerAndInventory(player, inventory);
            ShowInterface();
        }

        public static void ShowInterface()//이부분에서 문제가 생깁니다 다른곳에서 스위치 readline을 할때 잘못입력한경우 다시입력시 showinterface의 switch값으로 실행됩니다.
        {

            Console.Clear();

            Print.P("크로노 트리거");
            Print.P("\n가르디아 왕국에 오신 것을 환영합니다.");
            Print.P("1. 상태 보기 \n2. 인벤토리\n3. 상점\n4. 휴식\n5. 던전 입장");


            while (true)
            {

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1"://상태
                        player.ShowStatus();
                        break;

                    case "2"://인벤토리
                        inventory.ShowInventory();
                        break;
                    case "3"://상점
                        shop.ShowShop();
                        break;
                    case "4":
                        player.Rest();
                        break;
                    case "5":
                        EnterDungeon();
                        break;
                    default:
                        Print.P("잘못된입력입니다.");
                        break;
                }
            }
        }

        public static void EnterDungeon()
        {
            Console.Clear();
            Print.P("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Print.P("\n1. 쉬운 던전     | 방어력 5 이상 권장");
            Print.P("2. 일반 던전     | 방어력 11 이상 권장");
            Print.P("3. 어려운 던전    | 방어력 17 이상 권장");
            Print.P("0. 나가기");
            Dungeon dungeon = new Dungeon(player);
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    dungeon.DungeonChallenge(5, 1000, "쉬운 던전");
                    break;
                case "2":
                    dungeon.DungeonChallenge(11, 1700, "일반 던전");
                    break;
                case "3":
                    dungeon.DungeonChallenge(17, 2500, "어려운 던전");
                    break;
                case "0":
                    ShowInterface();
                    break;
                default:
                    Print.P("잘못된입력입니다.");
                    break;
            }
        }
    }

    public class Print
    {
        public static void P(string input)
        {
            Console.WriteLine(input);
        }
    }

    class Player
    {
        private Inventory inventory;
        public void SetInventory(Inventory inventory)
        {
            this.inventory = inventory;
        }

        public string Name { get; } = "크로노";
        public string Job { get; } = "용사";
        public int Attack { get; set; } = 10;
        public int AttackBonus
        {
            get
            {
                if (inventory != null)
                {
                    return inventory.BonusAttack();
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Defence { get; set; } = 5;
        public int DefenceBonus
        {
            get
            {
                if (inventory != null)
                {
                    return inventory.BonusDefence();
                }
                else
                {
                    return 0;
                }
            }
        }
        public int MaxHealth { get; set; } = 100;
        public int CurrentHealth { get; set; } = 100;
        public int Gold { get; set; } = 1000000;
        public int Level { get; set; } = 1;




        public void ShowStatus()
        {
            Console.Clear();
            Print.P($"이름 {Name} ({Job})");
            Print.P($"레벨 : {Level}");
            Print.P($"공격력 : {Attack + AttackBonus} (+{AttackBonus})");
            Print.P($"방어력 : {Defence + DefenceBonus} (+{DefenceBonus})");
            Print.P($"체력 : {MaxHealth}");
            Print.P($"골드 : {Gold}");
            Print.P("\n0. 나가기");

            string input = Console.ReadLine();
            switch (input)
            {
                case "0":
                    Program.ShowInterface();
                    break;
                default:
                    Print.P("잘못된입력입니다.");
                    Console.ReadKey();
                    ShowStatus();
                    break;
            }
        }

        public void Rest()
        {
            while (true)
            {
                if (Gold >= 500)
                {
                    int maxHP = MaxHealth;
                    CurrentHealth = maxHP;
                    Print.P("체력을 회복했습니다.");
                    Console.ReadKey();
                    Program.ShowInterface();
                }
                else
                {
                    Print.P("골드가 부족합니다.");
                    Console.ReadKey();
                    Program.ShowInterface();
                }
            }
        }

    }

    class Item
    {

        public string Name { get; set; }
        public int AttackWeapon { get; set; }
        public int DefenceArmor { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public bool IsWeapon { get; set; } = false;
        public bool IsEquipedWeapon { get; set; } = false;
        public bool IsArmor { get; set; } = false;
        public bool IsEquipedArmor { get; set; } = false;
        public Item(string name, int attackWeapon, int defenceArmor, int price, string description, bool isWeapon, bool isArmor)
        {
            Name = name;
            AttackWeapon = attackWeapon;
            DefenceArmor = defenceArmor;
            Price = price;
            Description = description;
            IsWeapon = isWeapon;
            IsArmor = isArmor;
        }

    }

    class Inventory
    {
        public List<Item> items = new List<Item>();

        public void ShowInventory()
        {
            Console.Clear();
            if (items.Count > 0)
            {
                Print.P("아이템 목록");
                for (int i = 0; i < items.Count; i++)
                {
                    Print.P($"{i + 1}. {items[i].Name}");
                }
                Print.P("\n\n1. 장착 관리 \n0. 나가기");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ManageEquipment();
                        break;
                    case "0":
                        Program.ShowInterface();
                        break;
                    default:
                        Print.P("잘못된입력입니다.");
                        Console.ReadKey();
                        ShowInventory();
                        break;
                }
            }
            else
            {
                Print.P("소지 중인 아이템이 없습니다.");
                Console.ReadKey();
                Program.ShowInterface();
            }
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            items.Remove(item);
        }

        public int BonusAttack()
        {
            int totalAttack = 0;
            foreach (var item in items)
            {
                if (item.IsEquipedWeapon)
                {
                    totalAttack += item.AttackWeapon;
                }
            }
            return totalAttack;
        }

        public int BonusDefence()
        {
            int totalDefence = 0;
            foreach (var item in items)
            {
                if (item.IsEquipedArmor)
                {
                    totalDefence += item.DefenceArmor;
                }
            }
            return totalDefence;
        }

        public void ManageEquipment()
        {
            while (true)
            {
                Console.Clear();
                Print.P("아이템 목록");

                for (int i = 0; i < items.Count; i++)
                {
                    string equippedMarker = (items[i].IsEquipedWeapon || items[i].IsEquipedArmor) ? "[E]" : "";
                    Print.P($"{i + 1}. {equippedMarker} {items[i].Name}");
                }

                Print.P("\n0. 나가기");
                Print.P("장착/해제할 아이템 번호를 입력하세요:");

                string input = Console.ReadLine();

                if (input == "0")
                {
                    Program.ShowInterface();

                }

                if (int.TryParse(input, out int choice) && choice > 0 && choice <= items.Count)
                {
                    Item selectedItem = items[choice - 1];

                    if (selectedItem != null && (selectedItem.IsEquipedWeapon || selectedItem.IsEquipedArmor))// seletedItem이 null값인지 먼저 확인하기때문에 이러한 조건식이 필요
                    {
                        Unequip(selectedItem);
                    }
                    else if (selectedItem != null)
                    {
                        Equip(selectedItem);
                    }
                    else
                    {
                        Print.P("잘못된 아이템 입니다.");
                        Console.ReadKey();
                    }

                    Console.Clear();
                }
                else
                {
                    Print.P("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ReadKey();
                }
            }
        }


        public void Equip(Item item)
        {
            if (item.IsWeapon)
            {
                foreach (Item item1 in items)
                {
                    item1.IsEquipedWeapon = false;
                }

                item.IsEquipedWeapon = true;
            }
            else if (item.IsArmor)
            {
                foreach (Item item1 in items)
                {
                    item1.IsEquipedArmor = false;
                }

                item.IsEquipedArmor = true;
            }

            Print.P($"{item.Name} 장착했습니다.");
            Console.ReadKey();
            ManageEquipment();
        }

        public void Unequip(Item item)
        {
            if (item.IsWeapon)
            {
                item.IsEquipedWeapon = false;
            }
            else if (item.IsArmor)
            {
                item.IsEquipedArmor = false;
            }

            Print.P($"{item.Name} 장착 해제했습니다.");
            Console.ReadKey();
            ManageEquipment();
        }
    }


    class Shop
    {
        private Player player;
        private Inventory inventory;
        public void SetPlayerAndInventory(Player player, Inventory inventory)
        {
            this.player = player;
            this.inventory = inventory;
        }

        private List<Item> shopItems = new List<Item>
    {
    new Item("바스타드 소드",10,0,100,"망나니의 검",true,false),
    new Item("아이언 소드",20,0,300,"철로 만든 검",true,false),
    new Item("다이아몬드 소드",30,0,700,"다이아몬드로 만든 검",true,false),
    new Item("그랜드리온",40,0,1500,"그랜과 리온",true,false),
    new Item("천 갑옷",0,5,50,"천으로 만든 갑옷",false,true),
    new Item("가죽 갑옷",0,10,100,"가죽으로 만든 갑옷",false,true),
    new Item("철 갑옷",0,15,300,"철로 만든 갑옷",false,true),
    new Item("다이아몬드 갑옷",0,20,1000,"다이아몬드로 만든 갑옷",false,true)
    };




        private bool[] purchased;
        public Shop()
        {
            purchased = new bool[shopItems.Count];

        }


        public void ShowShop()
        {
            Console.Clear();
            Print.P("상점 목록\n");

            for (int i = 0; i < shopItems.Count; i++)
            {
                Console.Write($"{i + 1}. {shopItems[i].Name} | ");

                if (shopItems[i].IsWeapon)
                {
                    Console.Write($"공격력 +{shopItems[i].AttackWeapon} | ");
                }
                else if (shopItems[i].IsArmor)
                {
                    Console.Write($"방어력 +{shopItems[i].DefenceArmor} | ");
                }

                Console.WriteLine($"{(purchased[i] ? "구매 완료" : "가격" + shopItems[i].Price) + " G"} | {shopItems[i].Description}");
            }

            Print.P("\n1. 구매하기\n2. 판매하기 \n0. 나가기");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    BuyItem();
                    break;
                case "2":
                    SellItem();
                    break;
                case "0":
                    Print.P("잘못된 입력입니다.");
                    Console.ReadKey();
                    ShowShop();
                    break;

            }


        }

        private void BuyItem()
        {
            while (true)
            {
                Console.Clear();

                Print.P("상점 목록\n");

                for (int i = 0; i < shopItems.Count; i++)
                {
                    Console.Write($"{i + 1}. {shopItems[i].Name} | ");

                    if (shopItems[i].IsWeapon)
                    {
                        Console.Write($"공격력 +{shopItems[i].AttackWeapon} | ");
                    }
                    else if (shopItems[i].IsArmor)
                    {
                        Console.Write($"방어력 +{shopItems[i].DefenceArmor} | ");
                    }

                    Console.WriteLine($"{(purchased[i] ? "구매 완료" : "가격" + shopItems[i].Price) + " G"} | {shopItems[i].Description}");
                }

                Print.P($"\n보유중인 골드 : {player.Gold}");
                Print.P("\n구매하려는 아이템 번호를 입력하세요\n0. 나가기");
                string input = Console.ReadLine();
                int.TryParse(input, out int choice);
                if (string.IsNullOrEmpty(input))
                {
                    Print.P("잘못된 입력입니다.");
                    Console.ReadKey();
                }

                if (choice > 0 && choice <= shopItems.Count)
                {
                    Item seletedItem = shopItems[choice - 1];
                    if (!purchased[choice - 1])
                    {
                        if (player.Gold >= shopItems[choice - 1].Price)
                        {
                            player.Gold -= shopItems[choice - 1].Price;
                            inventory.AddItem(shopItems[choice - 1]);
                            purchased[choice - 1] = true;
                            Print.P($"{seletedItem.Name}을 구매했습니다.");
                            Console.ReadKey();
                        }
                        else
                        {
                            Print.P("골드가 부족합니다.");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Print.P("이미 구매한 물품입니다.");
                        Console.ReadKey();
                    }
                }
                else if (choice == 0)
                {
                    Program.ShowInterface();
                }
                else
                {
                    Print.P("잘못된 입력입니다.");
                    Console.ReadKey();
                }
            }
        }

        private void SellItem()
        {

            while (true)
            {
                Console.Clear();
                if (inventory.items.Count > 0)
                {
                    Print.P("아이템 목록");
                    for (int i = 0; i < inventory.items.Count; i++)
                    {
                        Print.P($"{i + 1}. {inventory.items[i].Name}");
                    }

                    Print.P($"\n보유중인 골드 : {player.Gold} G");
                    Print.P("\n판매하려는 아이템 번호를 입력하세요 \n0. 나가기");
                    string input = Console.ReadLine();
                    int.TryParse(input, out int choice);
                    if (string.IsNullOrEmpty(input) || choice < 0 || choice > inventory.items.Count)
                    {
                        Print.P("잘못된 입력입니다.");
                        Console.ReadKey();
                    }

                    if (choice == 0)
                    {
                        Program.ShowInterface();
                    }
                    Print.P($"{inventory.items[choice - 1].Name}을 판매했습니다. {(inventory.items[choice - 1].Price * 100) / 85} G를 받았습니다. ");
                    player.Gold += (inventory.items[choice - 1].Price * 100) / 85;
                    inventory.RemoveItem(inventory.items[choice - 1]);
                    Console.ReadKey();
                }
                else
                {
                    Print.P("소지 중인 아이템이 없습니다.");
                    Console.ReadKey();
                }
            }
        }
    }

    class Dungeon
    {
        public int Difficulty { get; set; }
        private Player player;

        public Dungeon(Player player)
        {

            this.player = player;
        }

        Random rand = new Random();



        public void DungeonChallenge(int requiredDefence, int reward, string dungeonName)
        {
            int dice = rand.Next(0, 101);
            int hpLoss = 0;
            if (player.Defence + player.DefenceBonus < requiredDefence)
            {
                if (dice < 40)
                {
                    hpLoss = player.MaxHealth / 2;
                    player.CurrentHealth -= hpLoss;
                    Print.P("던전 실패!");
                    Console.ReadKey();
                    Program.EnterDungeon();
                }
                else
                {
                    Print.P("던전 성공!");
                    GetReward(reward);
                }
            }
            else if (player.Defence + player.DefenceBonus >= requiredDefence)
            {
                Print.P("던전 성공!");
                hpLoss = (dice * 15) / 100 + player.Defence + player.DefenceBonus - requiredDefence;
                player.CurrentHealth -= hpLoss;
                GetReward(reward);
            }
        }

        private void GetReward(int reward)
        {
            int dice = rand.Next(0, 20);

            player.Gold += reward + (reward * dice * (player.Attack + player.AttackBonus)) / 1000;
            Print.P($"{reward + (reward * dice * (player.Attack + player.AttackBonus)) / 1000} G를 보상으로 받았습니다.");
            Console.ReadKey();
            Program.EnterDungeon();
        }
    }
}
