# Hybrid Prefabs

### Setup

Add this package by using the Add package from git URL... + menu option

```json
"https://github.com/TomEganDev/DOTS.HybridPrefabs.git"
```

### How To Use

1. Create GameObject prefab with HybridPrefab component
2. Create Entity prefab with LinkHybridPrefab authoring component that references the hybrid prefab
3. Add a HybridPrefabCollection to your scene that contains all hybrid prefabs
4. Entities with LinkHybridPrefab will now automatically pair themselves to a hybrid prefab instance when instantiated

### Notes

Examples of usage will be uploaded soon