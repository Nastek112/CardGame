# CardGame
## Storage (Nazokat)

- Storage implementation: `CardGame.Storage.JsonGameStorage`
- File: `save.json` (по умолчанию, в рабочей директории)
- Интерфейс: `CardGame.Interfaces.IGameStorage` (Load/Save)

How to use:
- To load at app start: `var state = storage.Load() ?? GameState.NewGame(...)`
- To save: `storage.Save(state)`; recommend hooking to `AppDomain.CurrentDomain.ProcessExit` and `Console.CancelKeyPress`.

Tests:
- Run tests (xUnit): Test -> Run All Tests
