using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(ContentSizeFitter))]
[RequireComponent(typeof(RectTransform))]
public class GridFitter : UIBehaviour
{
    private GridLayoutGroup grid;
    private ContentSizeFitter fitter;
    bool initialized = false;
 
    protected override void Awake()
    {
        base.Awake();
        grid = GetComponent<GridLayoutGroup>();
        fitter = GetComponent<ContentSizeFitter>();
        initialized = true;
    }
 
    protected override void Start()
    {
        base.Start();
        CalculateGridSize();
    }
 
    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        CalculateGridSize();
    }
   
    public void CalculateGridSize()
    {
        // Seems OnRectTransformDimensionsChange gets called before Awake
        if (!initialized)
        {
            return;
        }
 
        RectTransform rectTrans = (RectTransform)transform;
 
        // Strech vertically
        if (fitter.verticalFit == ContentSizeFitter.FitMode.PreferredSize && fitter.horizontalFit == ContentSizeFitter.FitMode.Unconstrained)
        {
            int columns = (int)(rectTrans.rect.width / (grid.cellSize.x + grid.spacing.x));
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;
        }
        // Stretch horizontally
        else if (fitter.verticalFit == ContentSizeFitter.FitMode.Unconstrained && fitter.horizontalFit == ContentSizeFitter.FitMode.PreferredSize)
        {
            int rows = (int)(rectTrans.rect.height / (grid.cellSize.y + grid.spacing.y));
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = rows;
        }
        else
        {
            // Nothing to do
            return;
        }
    }
}
 