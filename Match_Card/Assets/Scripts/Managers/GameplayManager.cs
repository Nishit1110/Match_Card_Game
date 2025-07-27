using System;
using System.Collections;
using System.Collections.Generic;
using Nishit.Class;
using Nishit.Emums;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;
    public event Action<int> OnAttemptChanged;
    public event Action<float> OnTimeChanged;
    public event Action<bool> GameOver;
    public event Action<int> StreakChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private CardDatabase cardDatabase;

    [SerializeField]
    GridLayoutGroup CardsGrid;

    [SerializeField]
    RectTransform container;

    [SerializeField]
    int gridX;

    [SerializeField]
    int gridY;

    [SerializeField]
    ClickableCards clickableCardPrefab;

    ClickableCards FlippedCard = null;

    private List<ClickableCards> allCards = new();

    private float timer = 0f;

    private int score = 0;

    private int pairsFound = 0;

    private int comboStreak = 0;

    private int totalPairs = 0;

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsValidGridSize())
            return;

        ResizeGrid();
        totalPairs = gridX * gridY / 2;
        SpawnCards(totalPairs);
    }

    void Update()
    {
        if (isGameOver)
            return;

        timer += Time.deltaTime;
        OnTimeChanged?.Invoke(timer);
    }

    public void ResizeGrid()
    {
        float containerWidth = container.rect.width;
        float containerHeight = container.rect.height;

        int columns = gridX;
        int rows = gridY;

        // Calculate max spacing based on container and grid
        float spacingX = (float)containerWidth / (columns + 1);
        float spacingY = (float)containerHeight / (rows + 1);
        float spacing = Mathf.Min(spacingX, spacingY) * 0.2f; // 10% of available gap

        // Now recalculate available space
        float totalSpacingX = spacing * (columns + 1);
        float totalSpacingY = spacing * (rows + 1);

        float availableWidth = containerWidth - totalSpacingX;
        float availableHeight = containerHeight - totalSpacingY;

        float cellSize = Mathf.Min(availableWidth / columns, availableHeight / rows);

        // Apply
        CardsGrid.spacing = new Vector2(spacing, spacing);
        CardsGrid.padding = new RectOffset((int)spacing, (int)spacing, (int)spacing, (int)spacing);
        CardsGrid.cellSize = new Vector2(cellSize, cellSize);
    }

    private void SpawnCards(int pairCount)
    {
        // Step 1: Get enum values
        CardValues[] allValues = (CardValues[])System.Enum.GetValues(typeof(CardValues));

        if (pairCount > allValues.Length)
        {
            Debug.LogError(
                "Not enough unique CardValues to generate the requested number of pairs."
            );
            return;
        }

        // Step 2: Randomly pick N unique values
        List<CardValues> selectedValues = new List<CardValues>();
        List<int> usedIndexes = new List<int>();

        while (selectedValues.Count < pairCount)
        {
            int randIndex = Random.Range(0, allValues.Length);
            if (!usedIndexes.Contains(randIndex))
            {
                usedIndexes.Add(randIndex);
                selectedValues.Add(allValues[randIndex]);
            }
        }

        // Step 3: Duplicate each value to create pairs
        List<CardValues> spawnPool = new List<CardValues>();
        foreach (var val in selectedValues)
        {
            spawnPool.Add(val);
            spawnPool.Add(val);
        }

        // Step 4: Shuffle the list
        for (int i = 0; i < spawnPool.Count; i++)
        {
            int randomIndex = Random.Range(i, spawnPool.Count);
            (spawnPool[i], spawnPool[randomIndex]) = (spawnPool[randomIndex], spawnPool[i]);
        }

        // Step 5: Spawn the cards
        foreach (CardValues value in spawnPool)
        {
            ClickableCards card = Instantiate(clickableCardPrefab, CardsGrid.transform);
            card.Init(value, this, GetCardSprite(value));
            allCards.Add(card);
        }
    }

    public void CardFlipped(ClickableCards card)
    {
        if (FlippedCard != null && FlippedCard != card)
        {
            if (FlippedCard.cardValue == card.cardValue)
            {
                // ✅ Match found
                FlippedCard.CardMatched();
                card.CardMatched();

                // Increase streak and score
                int comboPoints = (int)Mathf.Pow(2, comboStreak); // 2^streak
                score += comboPoints;

                OnScoreChanged?.Invoke(score);
                OnAttemptChanged?.Invoke(1);

                pairsFound++;
                comboStreak++; // streak increases on correct match

                if (comboStreak > 1)
                {
                    StreakChanged?.Invoke(comboStreak);
                    Debug.Log($"Combo Streak: {comboStreak}");
                }

                if (pairsFound >= totalPairs) // or use a separate match counter
                {
                    isGameOver = true;
                    GameOver?.Invoke(true);
                    Debug.Log("Game Over! All pairs matched.");
                }
            }
            else
            {
                FlippedCard.ResetCardToDefault();
                card.ResetCardToDefault();

                OnAttemptChanged?.Invoke(1);

                comboStreak = 0; // 🔁 reset combo streak on fail
            }

            FlippedCard = null;
        }
        else
        {
            FlippedCard = card;
        }
    }

    private bool IsValidGridSize()
    {
        return (gridX * gridY) % 2 == 0;
    }

    Sprite GetCardSprite(CardValues value)
    {
        return cardDatabase.GetCardSprite(value);
    }
}
