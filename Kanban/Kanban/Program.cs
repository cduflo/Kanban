using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban
{
    class Program
    {
        static void Main(string[] args)
        {
            bool repeat = true;
 
            while (repeat)
            {
                try
                {
                    GetListing();
                    Console.WriteLine("\n\n");
                    Console.WriteLine("Please enter the integer of your choice:\n1. Add a list\n2. Add a card.\n3.Delete a list\n4.Delete a card\n'quit' to exit");
                    var choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine("Please enter the name of the new List.");
                            var list = Console.ReadLine();
                            NewList(list);
                            break;
                        case "2":
                            Console.WriteLine("Please enter the list you want to add a card to.");
                            var listadd = Console.ReadLine();
                            Console.WriteLine("Please enter the name of the new Card.");
                            var cardadd = Console.ReadLine();
                            NewCard(listadd, cardadd);
                            break;
                        case "3":
                            Console.WriteLine("Please enter the name of List to delete.");
                            var listDel = Console.ReadLine();
                            DelList(listDel);
                            break;
                        case "4":
                            Console.WriteLine("Please enter the Card you want to delete.");
                            var cardDel = Console.ReadLine();
                            DelCard(cardDel);
                            break;
                        case "quit":
                            repeat = false;
                            break;
                        default:
                            Console.WriteLine("No valid selection made.");
                            break;
                    }
                }
                catch
                {
                    Console.WriteLine("There was an error with your selection, please enter an integer for your choice or 'quit' to exit.");
                }
                Console.WriteLine("\n\n");
            }

            Console.ReadLine();
        }

        private static void GetListing()
        {
            using (var db = new KanbanEntities())
            {
                foreach (var list in db.Lists)
                {
                    Console.WriteLine(list.Name);

                    var subcards = db.Cards.Where(u => u.ListID == list.ListID);
                    foreach (var card in subcards)
                    {
                        Console.WriteLine("\t" + card.Text);
                    }
                }
            }
        }

        private static void NewList(string listname)
        {
            using (var db = new KanbanEntities())
            {
                var newrow = db.Set<List>();
                newrow.Add(new List { Name = listname, CreatedDate = DateTime.Now });
                db.SaveChanges();
                Console.WriteLine(listname + " added!");
            }
        }

        private static void NewCard(string listname, string cardname)
        {
            int sList = 0;
            using (var db = new KanbanEntities())
            {
                var selList = db.Lists.Where(u => u.Name == listname);//.Select(u => u.ListID);
                foreach (var x in selList)
                {
                    sList = x.ListID;
                }
                var newrow = db.Set<Card>();
                newrow.Add(new Card { ListID = sList, CreatedDate = DateTime.Now, Text = cardname });
                db.SaveChanges();
                Console.WriteLine(cardname + " added to " + listname);
            }
        }

        private static void DelList(string list)
        {
            using (var db = new KanbanEntities())
            {
                var listDelete = db.Lists.Where(u => u.Name == list);

                foreach (var u in listDelete)
                {
                    db.Lists.Remove(u);
                }
                db.SaveChanges();
            }
        }

        private static void DelCard(string text)
        {
            Card d = null;
            using (var db = new KanbanEntities())
            {
                var delCard = db.Cards.Where(u => u.Text == text);
                foreach (var x in delCard)
                {
                    d = x;
                }
                db.Cards.Remove(d);
                db.SaveChanges();
            }
        }
    }
}
