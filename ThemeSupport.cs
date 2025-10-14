using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace K.ThemeSupport {
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class ThemedControlInfo {
        public Control Control { get; set; }
        public Dictionary<string, (object dark, object light)> PropertyMap { get; set; } = new Dictionary<string, (object, object)>();

        public void ApplyTheme(bool isDarkMode) {
            foreach (var kvp in PropertyMap) {
                var prop = Control.GetType().GetProperty(kvp.Key);
                if (prop == null || !prop.CanWrite) continue;
                var value = isDarkMode ? kvp.Value.dark : kvp.Value.light;
                if (prop.PropertyType.IsAssignableFrom(value.GetType())) prop.SetValue(Control, value);
            }
        }
    }

    public static class ThemeManager {
        private static readonly List<ThemedControlInfo> _controls = new List<ThemedControlInfo>();
        public static bool IsDarkMode { get; private set; } = false;

        // Register with a single property
        // If the control is already registered, this method add new property to PropertyMap
        public static void Register(Control control, string propertyName, object darkValue, object lightValue) {
            var darkType = darkValue?.GetType();
            var lightType = lightValue?.GetType();
            var prop = control.GetType().GetProperty(propertyName);

            if (prop == null) throw new ArgumentException($"Property '{propertyName}' was not exist on {control.GetType().Name}!");
            if (!prop.CanWrite) throw new ArgumentException($"Property '{propertyName}' on {control.GetType().Name} can't write!");
            if (darkType == null || lightType == null) throw new ArgumentException($"Value of '{propertyName}' can't null!");
            if (!prop.PropertyType.IsAssignableFrom(darkType) || !prop.PropertyType.IsAssignableFrom(lightType)) throw new ArgumentException($"Type of '{propertyName}' must be '{prop.PropertyType.Name}' on {control.GetType().Name}!");

            var existing = _controls.Find(c => c.Control == control);
            if (existing == null) {
                existing = new ThemedControlInfo { Control = control };
                _controls.Add(existing);
            }

            existing.PropertyMap[propertyName] = (darkValue, lightValue);
            existing.ApplyTheme(IsDarkMode);
        }

        // Register with multi properties
        // If the control is already registered, this method overrides its current PropertyMap with a new one
        public static void Register(Control control, Dictionary<string, (object dark, object light)> propertyMap) {
            foreach (var kvp in propertyMap) {
                var darkType = kvp.Value.dark?.GetType();
                var lightType = kvp.Value.light?.GetType();
                var prop = control.GetType().GetProperty(kvp.Key);

                if (prop == null) throw new ArgumentException($"Property '{kvp.Key}' was not exist on {control.GetType().Name}!");
                if (!prop.CanWrite) throw new ArgumentException($"Property '{kvp.Key}' on {control.GetType().Name} can't write!");
                if (darkType == null || lightType == null) throw new ArgumentException($"Value of '{kvp.Key}' can't null!");
                if (!prop.PropertyType.IsAssignableFrom(darkType) || !prop.PropertyType.IsAssignableFrom(lightType)) throw new ArgumentException($"Type of '{kvp.Key}' must be '{prop.PropertyType.Name}' on {control.GetType().Name}!");
            }

            var existing = _controls.Find(c => c.Control == control);
            if (existing == null) {
                existing = new ThemedControlInfo { Control = control };
                _controls.Add(existing);
            }

            existing.PropertyMap = propertyMap;
            _controls.Add(existing);
            existing.ApplyTheme(IsDarkMode);
        }

        // Switches the mode between light and dark
        public static void ToggleTheme() {
            IsDarkMode = !IsDarkMode;
            foreach (var info in _controls) info.ApplyTheme(IsDarkMode);
        }

        // Specify the mode explicitly: true → Dark mode, false → Light mode
        public static void SetTheme(bool darkMode) {
            IsDarkMode = darkMode;
            foreach (var info in _controls) info.ApplyTheme(IsDarkMode);
        }

        // Reload current mode for all registered controls
        public static void Refresh() {
            foreach (var info in _controls) info.ApplyTheme(IsDarkMode);
        }

        // Reload current mode for a specific control
        public static void Refresh(Control control) {
            var info = _controls.Find(c => c.Control == control);
            if (info != null) info.ApplyTheme(IsDarkMode);
        }
    }
}