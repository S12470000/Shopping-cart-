using System;
using System.Collections.Generic;
using System.Linq;

// Product base class
abstract class ProductBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }

    public ProductBase(int id, string name, double price, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }

    public abstract void DisplayInfo();
}

// Electronics Product
class ElectronicsProduct : ProductBase
{
    public int WarrantyInMonths { get; set; }

    public ElectronicsProduct(int id, string name, double price, int warranty)
        : base(id, name, price, "Electronics")
    {
        WarrantyInMonths = warranty;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[E] ID: {Id}, Name: {Name}, Price: {Price:C}, Warranty: {WarrantyInMonths} months");
    }
}

// Grocery Product
class GroceryProduct : ProductBase
{
    public DateTime ExpiryDate { get; set; }

    public GroceryProduct(int id, string name, double price, DateTime expiry)
        : base(id, name, price, "Grocery")
    {
        ExpiryDate = expiry;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[G] ID: {Id}, Name: {Name}, Price: {Price:C}, Expiry: {ExpiryDate:dd-MM-yyyy}");
    }
}

// Cart Item
class CartItem
{
    public ProductBase Product { get; set; }
    public int Quantity { get; set; }

    public double TotalPrice => Product.Price * Quantity;
}

// Interface for Cart Operations
interface ICartOperations
{
    void AddToCart(int productId, int quantity);
    void RemoveFromCart(int productId);
    void ViewCart();
    void Checkout();
}

// Shopping Cart class
class ShoppingCart : ICartOperations
{
    private List<ProductBase> productList = new List<ProductBase>();
    private List<CartItem> cartItems = new List<CartItem>();

    public ShoppingCart()
    {
        // Electronics
        productList.Add(new ElectronicsProduct(1, "Laptop", 45000, 24));
        productList.Add(new ElectronicsProduct(2, "Smartphone", 25000, 12));
        productList.Add(new ElectronicsProduct(3, "Wireless Earbuds", 3500, 6));
        productList.Add(new ElectronicsProduct(4, "Smart Watch", 8000, 12));
        productList.Add(new ElectronicsProduct(5, "LED TV 42\"", 32000, 18));
        productList.Add(new ElectronicsProduct(6, "Bluetooth Speaker", 2000, 6));
        productList.Add(new ElectronicsProduct(7, "Gaming Console", 40000, 24));
        productList.Add(new ElectronicsProduct(8, "Digital Camera", 30000, 12));

        // Grocery
        productList.Add(new GroceryProduct(9, "Rice (1kg)", 60, DateTime.Now.AddMonths(6)));
        productList.Add(new GroceryProduct(10, "Milk (1L)", 45, DateTime.Now.AddDays(7)));
        productList.Add(new GroceryProduct(11, "Eggs (12 pcs)", 70, DateTime.Now.AddDays(10)));
        productList.Add(new GroceryProduct(12, "Atta (5kg)", 210, DateTime.Now.AddMonths(4)));
        productList.Add(new GroceryProduct(13, "Salt (1kg)", 20, DateTime.Now.AddMonths(12)));
        productList.Add(new GroceryProduct(14, "Cooking Oil (1L)", 150, DateTime.Now.AddMonths(9)));
        productList.Add(new GroceryProduct(15, "Sugar (1kg)", 45, DateTime.Now.AddMonths(6)));
        productList.Add(new GroceryProduct(16, "Tea Powder (500g)", 180, DateTime.Now.AddMonths(8)));
        productList.Add(new GroceryProduct(17, "Toor Dal (1kg)", 130, DateTime.Now.AddMonths(5)));
    }

    public void DisplayProducts()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n📦 Available Products (Grouped by Category):");
        Console.ResetColor();

        var grouped = productList.GroupBy(p => p.Category);
        foreach (var group in grouped)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n🔸 {group.Key}:");
            Console.ResetColor();

            foreach (var product in group)
            {
                product.DisplayInfo();
            }
        }
    }

    public void AddToCart(int productId, int quantity)
    {
        var product = productList.FirstOrDefault(p => p.Id == productId);
        if (product == null || quantity <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Invalid product ID or quantity.");
            Console.ResetColor();
            return;
        }

        var existing = cartItems.FirstOrDefault(c => c.Product.Id == productId);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            cartItems.Add(new CartItem { Product = product, Quantity = quantity });
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ Added to cart.");
        Console.ResetColor();
    }

    public void RemoveFromCart(int productId)
    {
        var item = cartItems.FirstOrDefault(c => c.Product.Id == productId);
        if (item != null)
        {
            cartItems.Remove(item);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("🗑️ Removed from cart.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Item not found in cart.");
        }
        Console.ResetColor();
    }

    public void ViewCart()
    {
        if (!cartItems.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("🛒 Cart is empty.");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n🛍️ Your Cart:");
        Console.ResetColor();

        foreach (var item in cartItems)
        {
            Console.WriteLine($"ID: {item.Product.Id}, Name: {item.Product.Name}, Qty: {item.Quantity}, Price: {item.TotalPrice:C}");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"💵 Total: {CalculateTotal():C}");
        Console.ResetColor();
    }

    public double CalculateTotal()
    {
        return cartItems.Sum(i => i.TotalPrice);
    }

    public void Checkout()
    {
        try
        {
            if (!cartItems.Any())
                throw new InvalidOperationException("Cart is empty. Cannot proceed to checkout.");

            ViewCart();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("🧾 Checking out... Thank you for shopping!");
            Console.ResetColor();

            cartItems.Clear();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚠️ Error during checkout: {ex.Message}");
            Console.ResetColor();
        }
    }
}

// Main Program
class Program
{
    static void Main()
    {
        ShoppingCart cart = new ShoppingCart();
        bool exit = false;

        while (!exit)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n📋 Menu:");
            Console.ResetColor();

            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Add to Cart");
            Console.WriteLine("3. Remove from Cart");
            Console.WriteLine("4. View Cart");
            Console.WriteLine("5. Checkout");
            Console.WriteLine("6. Exit");
            Console.Write("Enter choice: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    cart.DisplayProducts();
                    break;

                case "2":
                    Console.Write("Enter Product ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int pid))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Invalid Product ID.");
                        Console.ResetColor();
                        break;
                    }

                    Console.Write("Enter Quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int qty))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Invalid Quantity.");
                        Console.ResetColor();
                        break;
                    }

                    cart.AddToCart(pid, qty);
                    break;

                case "3":
                    Console.Write("Enter Product ID to remove: ");
                    if (!int.TryParse(Console.ReadLine(), out int rid))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Invalid Product ID.");
                        Console.ResetColor();
                        break;
                    }
                    cart.RemoveFromCart(rid);
                    break;

                case "4":
                    cart.ViewCart();
                    break;

                case "5":
                    cart.Checkout();
                    break;

                case "6":
                    exit = true;
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❗ Invalid choice.");
                    Console.ResetColor();
                    break;
            }
        }

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("👋 Exiting... Goodbye!");
        Console.ResetColor();
    }
}
