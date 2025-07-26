using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    GridLayoutGroup CardsGrid;

    [SerializeField]
    RectTransform container;

    [SerializeField]
    int gridX;

    [SerializeField]
    int gridY;

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResizeGrid();
        }
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

}
