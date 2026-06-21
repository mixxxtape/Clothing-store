# Ravel — Alternative Fashion Store

A full-featured e-commerce web application for alternative fashion built with ASP.NET Core MVC 8.0, Entity Framework Core, and PostgreSQL. The project includes a complete shopping experience for customers and a comprehensive admin panel for store management.


---
## UML and Database diagrams
[View PDF](/img/ClothingStore_UML_ER.pdf")

## Screenshots

## Catalog
<img width="1904" height="903" alt="image" src="https://github.com/user-attachments/assets/2ac15594-781c-44db-b689-3d8d6edc1458" />

---

 ## System from admin view 
<img width="1900" height="911" alt="image" src="https://github.com/user-attachments/assets/5a5c7d12-5029-4ca9-8ef3-9776c29d08e5" />

---

## Analytics
<img width="1906" height="910" alt="image" src="https://github.com/user-attachments/assets/f0da1cae-8777-41d5-88da-081abdc80a8d" />

---

## Users wishlist
<img width="1603" height="630" alt="image" src="https://github.com/user-attachments/assets/38a4f1d6-20f1-47e9-b69f-2a86a034e43f" />

---

## Product details
<img width="1291" height="899" alt="image" src="https://github.com/user-attachments/assets/ad51911d-de2b-4c9f-90f6-10092dca64a9" />


---

## Features

### Guest
- Browse the full product catalogue with product images, style tags, sizes and prices
- Filter products by **category**, **style**, and **price range**
- Fuzzy keyword search using the Levenshtein distance algorithm — finds results even with typos (e.g. "bluse" → "blouse")
- View detailed product pages with size availability and stock count
- Browse styles page with descriptions and images
- Register and log in to unlock full functionality

### Customer
- **Cart** — add products with size and quantity selection; update quantities; remove items; persistent across sessions (stored in DB)
- **Wishlist** — save products for later; move items to cart; toggle wishlist status directly from the catalogue
- **Checkout** — place orders with a delivery address (auto-filled from profile default address)
- **Orders** — view full order history with itemized details (product name, size, quantity, price at time of purchase); cancel pending orders
- **Order status tracking** — statuses: Pending → Processing → Shipped → Delivered / Cancelled
- **Style Quiz** — a 10-question interactive quiz shown one question at a time; each answer is weighted by style using a scoring algorithm (`Dictionary<int, int>`); results redirect to a filtered catalogue for the recommended style
- **Product reviews** — leave a rating (1–5 stars) and optional comment; edit or delete your own review; reviews are only available after receiving a delivery (status = Delivered)
- **Profile** — edit first name, last name, phone number, bio, and default delivery address; change password; logout

### Authentication & Security
- Registration with **email confirmation** via Gmail SMTP — login is blocked until email is confirmed
- **Password reset** via one-time email link
- Secure password hashing via ASP.NET Identity
- Two roles: **user** and **admin**

### Admin Panel
**Products**
- View all products in a list (including soft-deleted ones, highlighted in red)
- Create products: name, description, price, style, category, image (file upload or URL), sizes with individual stock quantities
- Edit products: update all fields; sizes used in existing orders are protected from deletion
- Soft-delete products (IsDeleted flag) — they disappear from the catalogue but remain in order history
- **Bulk import** products from `.xlsx` Excel files with full row-by-row validation (checks required fields, price range, existing categories and styles, duplicates)
- **Export** the full product catalogue to `.xlsx`

**Categories & Sizes**
- Full CRUD for product categories (e.g. Dress, Jacket, Earrings)
- Full CRUD for sizes (e.g. S, M, L, XL, Onesize)

**Styles**
- Full CRUD for fashion styles (e.g. Lolita, Dark Academia, Cottagecore)
- Two images per style: one for the styles list card and one for the style detail page
- Style detail page shows all active products of that style

**Orders**
- View all customer orders with user name, email, delivery address, date, itemized contents and total
- Change order status: Processing → Shipped → Delivered → Cancelled
- Stock is automatically returned to inventory when an order is cancelled
- Delete orders (e.g. old cancelled ones)

**Users**
- View all registered users with name, email, phone, roles and email confirmation status
- Manually confirm a user's email
- Edit user roles (assign/remove admin or user role)
- Delete user accounts

**Quiz**
- Create, edit, and delete the quiz and its questions
- Create, edit, and delete answers for each question
- Assign one or more styles to each answer (used in the scoring algorithm)

**Analytics**
- Visual dashboards built with Chart.js for store statistics and insights
- Product distribution analysis by **style** and **category**
- Average product price comparison across categories
- Visualization of style popularity based on users' quiz results
- Interactive charts providing administrators with quick insights into customer preferences and catalogue composition


---

## Architecture

The project follows a layered architecture with elements of Domain-Driven Design:
ClothingStoreMVC.Domain         → Entities, Aggregates (IAggregateRoot), Interfaces

ClothingStoreMVC.Infrastructure → DbContexts, EF Configurations, Services (Email, Import, Export)

ClothingStoreMVC.WebMVC         → Controllers, ViewModels, Razor Views

Two separate database contexts:
- `ClothingStoreContext` — domain data (products, orders, cart, wishlist, reviews, quiz)
- `IdentityContext` — ASP.NET Identity (users, roles, tokens)

Key database design decisions:
- Global query filter `HasQueryFilter(p => !p.IsDeleted)` for soft-delete pattern on products
- `OnDelete(DeleteBehavior.Restrict)` on OrderItem → Product and OrderItem → ProductSize to prevent cascade deletion of order history
- `ProductName` and `Price` stored directly in `OrderItem` to preserve order history after product deletion

---

## Tech Stack

| Layer | Technology |
|---|---|
| Language | C# / .NET 8.0 |
| Framework | ASP.NET Core MVC 8.0 |
| ORM | Entity Framework Core (Code-First, Migrations) |
| Database | PostgreSQL |
| Auth | ASP.NET Identity |
| Frontend | Razor Views, Bootstrap, custom CSS |
| Email | Gmail SMTP via SmtpClient |
| Excel | ClosedXML |
| Version Control | Git / GitHub |

---

## Getting Started

1. Clone the repository
2. Configure `appsettings.json`:
   - PostgreSQL connection strings for `ClothingStoreContext` and `IdentityContext`
   - Gmail SMTP credentials under `EmailSettings`
3. Apply migrations:
```bash
Update-Database -Context ClothingStoreContext
Update-Database -Context IdentityContext
```
4. Run the application — the admin account is seeded automatically on first launch

---

