# ThemeManager for WinForms

A lightweight and flexible theme manager for WinForms applications — written entirely in C#, packed into a single `.cs` file.  
Easily switch between Dark and Light modes, and apply custom styling to any control with full control over properties like `BackColor`, `ForeColor`, `Font`, `BorderColor`, `Text`, and more.

---

## ✨ Features

- ✅ Supports any WinForms control — including third-party controls like Guna.UI2
- 🎨 Apply theme-specific values for multiple properties (Color, Font, int, bool, string...)
- 🔍 Validates property names and types at registration time to prevent silent errors
- 🔁 Toggle between Dark and Light mode with a single call
- 🔧 Refresh theme for individual controls on demand
- 📦 No dependencies, no DLLs — just drop in and use

---

## 🚀 Getting Started

### 1. Add the file

Copy `ThemeManager.cs` into your WinForms project.

### 2. Register controls with theme-specific properties
First, you must declare the namespace:
```csharp
using K.ThemeSupport;
```
Then, register control need to manager:
- Register with multi properties  
  If the control is already registered, this method overrides its current PropertyMap with a new one
```csharp
ThemeManager.Register(label1, new Dictionary<string, (object, object)> {
    { "ForeColor", (Color.White, Color.Black) },
    { "BackColor", (Color.Black, Color.White) },
    { "Font", (new Font("Segoe UI", 12), new Font("Segoe UI", 10)) },
    { "Text", ("Dark Mode", "Light Mode") }
    //...
});
```
- Or you can register with a single property  
  If the control is already registered, this method add new property to PropertyMap
```csharp
ThemeManager.Register(label1, "ForeColor", Color.White, Color.Black);
ThemeManager.Register(label1, "BackColor", Color.Black, Color.White);
ThemeManager.Register(label1, "Font", new Font("Segoe UI", 12), new Font("Arial", 10));
ThemeManager.Register(label1, "Text", "Dark Mode", "Light Mode");
//...
```
### 3. Call methods to toggle theme
The following mode-switching methods are available for use:
- Switches the mode between light and dark:
```csharp
ThemeManager.ToggleTheme();
```
- Specify the mode explicitly: true → Dark mode, false → Light mode:
```csharp
ThemeManager.SetTheme(bool darkMode);
```
- Reload current mode for all registered controls:
```csharp
ThemeManager.Refresh();
```
- Reload current mode for a specific control:
```csharp
ThemeManager.Refresh(Control control);
```
