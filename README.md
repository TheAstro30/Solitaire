# Solitaire
A simple implementation of the classic game "Klondike", or "Patience"

Make sure to copy the "data" folder from the "Debug' folder to the "Release" folder, or the graphics won't work.

There is an excluded class called "DeckBuilder.cs" in "Classes/Helpers" that builds the graphics data files from resource images (which are also not included, but are located in the "Images" folder). These must be added as an existing resource, and call DeckBuilder.BuildDeck(); from somewhere at start up. A good place is void Main in "Program.cs" before the main form is even loaded.
