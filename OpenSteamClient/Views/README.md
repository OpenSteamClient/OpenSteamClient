# Views
Views define the UI. They don't define any logic, that is driven by properties and methods in ViewModels, which all views should bind to.
In something like a login window with multiple possible "sub-views" like an account picker and credential input, you can use a property in a ViewModel to define what View should be visible. Then use a ContentControl and bind it's Content property to that ViewModel.

# Windows vs Dialogs
Windows are non-blocking, meaning they don't prevent input to the window that summoned them.
Dialogs are blocking, until the user takes action by closing the dialog, where they will stop input to the window that summoned them. 

# I don't know where I should put my View
Just place it in the Views folder if unsure. It'll be handled when reviewing your PR, or kept there if needed.