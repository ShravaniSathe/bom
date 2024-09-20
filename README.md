# BOM System - Backend APIs

This repository contains the backend API implementation for the Bill of Materials (BOM) system. The application is designed to manage and visualize the structure of products, sub-assemblies, raw materials, and bought-out items in a hierarchical BOM structure.

# Features
- **Create BOM Tree**: Users can create BOM structures by defining parent-child relationships between various components, raw materials, and sub-assemblies.
- **Manage Products and Components**: Add, update, and manage items such as final products, raw materials, sub-assemblies, and bought-out items.
- **BOM Visualization**: Provides API endpoints that allow the frontend to visualize the hierarchical BOM tree.
- **RESTful APIs**: The backend follows a controller-manager-repository pattern, using Dapper for database access, with dependency injection for better code maintainability.

# BOM Concept :
The Bill of Materials (BOM) is a comprehensive list that outlines all the raw materials, components, and sub-assemblies required to manufacture a product. It represents a hierarchical structure that traces every part and material needed, from the top-level product down to the smallest components. The BOM can contain both manufactured parts (which require further processing or assembly) and bought-out parts (items purchased from suppliers).

# Hierarchical Structure of a BOM
In a BOM, you start with the final product (known as the top-level assembly), which is then broken down into its constituent parts, which may be:
- Raw materials that are either used directly in manufacturing or processed further.
- Sub-assemblies, which are intermediate assemblies or components made by combining raw materials.
- Bought-out items, which are parts purchased from suppliers and used directly without modification.
  
The BOM is often visualized as a tree-like structure where:
- The root node is the final product (e.g., a Sedan Car).
- The child nodes are the components or sub-assemblies of that product.
- This breakdown continues until all leaf nodes (the end components of the BOM) are bought-out items.

# BOM Breakdown Example
Consider a scenario where you need to manufacture a Sedan Car. This car is the final product, and it consists of several parts like the engine, transmission, wheels, seats, etc. Some of these parts may be sub-assemblies made from further components (like the Engine Assembly), and others may be bought-out items (like Alloy Wheels).

# The Key Concept: Continuation Until All Leaf Nodes are Bought Out
In the BOM process, you continue breaking down each component until you reach the leaf nodes. A leaf node is a part that does not need further breakdown—it's either a raw material that is processed in-house or a bought-out item.

# Example of a Complete BOM
**Sedan Car (1 Unit)** - [Luxury, Manufactured]  
│  
├── **Piston (4 Pieces)** - [Standard, Manufactured]  
│   ├── **Cylinder Head (1 Piece)** - [Premium, Manufactured]  
│   │   └── **Camshaft (1 Piece)** - [Premium, Bought-Out]  
│   └── **Sub-Assembly: Engine Assembly (1 Unit)** - [Manufactured]  
│  
├── **Sub-Assembly: Transmission (1 Unit)** - [Manufactured]  
│  
├── **Alloy Wheels (4 Sets)** - [Luxury, Bought-Out]  
│  
├── **Leather Seats (1 Set)** - [Premium, Bought-Out]  
│  
└── **Clutch (1 Piece)** - [Standard, Bought-Out]

- Root Node: The top-level product (Sedan Car).
- Sub-Assemblies: Components like the Engine Assembly or Transmission, which are manufactured and need further breakdown.
- Bought-Out Items: The leaf nodes that are purchased and used directly (like Alloy Wheels, Leather Seats, Camshaft, etc.).
- Raw Materials: Materials used in the sub-assemblies, which may or may not require further processing.

 # Packages :
- AutoMapper(13.0.1)
- Dapper(2.1.35)
- Dapper.Contrib(2.0.78)
- Microsoft.EntityFrameworkCore(8.0.8)
- Microsoft.EntityFrameworkCore.SqlServer(8.0.8)
- Microsoft.EntityFrameworkCore.Tools(8.0.8)
- Microsoft.VisualStudio.Web.CodeGeneration.Design(8.0.5)
- NLog(5.3.4)
- NLog.Web.AspNetCore(5.3.13)
- Swashbuckle.AspNetCore(6.4.0)
- System.Data.SqlClient(4.8.6)
  
*Set up the database connection string in appsettings.json*
