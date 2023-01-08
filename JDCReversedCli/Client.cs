using JDCReversed;
using JDCReversed.Packets;

namespace JDCReversedCli;

public class Client : WebSocketConnection
{
    public enum NavigationAction
    {
        SwipeLeft, SwipeRight, SwipeUp, SwipeDown, ActionLeft, ActionRight
    }
    
    private int _currentRow;
    private int _currentItem;
    private int _currentAction;

    private int _rowCount;
    private int _itemCount;
    private int _actionCount;
    
    private JdPhoneCarouselRow[]? _rows;
    
    public Client(string host) : base(host)
    {
    }

    public override void HandleResponse(JdObject? response)
    {
        switch (response)
        {
            case JdPhoneUiSetupData data:
            {
                _rows = data.SetupData?.MainCarousel?.Rows; 
                if (_rows != null) _rowCount = _rows.Length;
                
                if (data.InputSetup?.CarouselPosSetup != null)
                {
                    _currentRow = data.InputSetup.CarouselPosSetup.RowIndex;
                    var rowItems = _rows?[_currentRow].Items;
                    if (rowItems != null) _itemCount = rowItems.Length;
                    _currentItem = data.InputSetup.CarouselPosSetup.ItemIndex;
                    var actionItems = rowItems?[_currentItem].Actions;
                    if (actionItems != null) _actionCount = actionItems.Length;
                    _currentAction = data.InputSetup.CarouselPosSetup.ActionIndex;
                }

                break;
            }
            case JdInputSetupConsoleCommandData data:
            {
                if (data.CarouselPosSetup != null)
                {
                    _currentRow = data.CarouselPosSetup.RowIndex;
                    var rowItems = _rows?[_currentRow].Items;
                    if (rowItems != null) _itemCount = rowItems.Length;
                    _currentItem = data.CarouselPosSetup.ItemIndex;
                    var actionItems = rowItems?[_currentItem].Actions;
                    if (actionItems != null) _actionCount = actionItems.Length;
                    _currentAction = data.CarouselPosSetup.ActionIndex;
                }

                break;
            }
        }
    }

    public async Task Navigate(NavigationAction action)
    {
        JdObject? request = null;

        switch (action)
        {
            case NavigationAction.ActionLeft:
            case NavigationAction.ActionRight:
            {
                if (_actionCount == 0)
                    break;
                
                _currentAction = (_actionCount + _currentAction + (action == NavigationAction.ActionRight ? 1 : -1)) %
                                 _actionCount;
                request = new JdChangeActionPhoneCommandData
                {
                    RowIndex = _currentRow,
                    ItemIndex = _currentItem,
                    ActionIndex = _currentAction
                };
                break;
            }
            case NavigationAction.SwipeUp:
            case NavigationAction.SwipeDown:
            {
                if (_rowCount == 0)
                    break;
                
                _currentRow = (_rowCount + _currentRow + (action == NavigationAction.SwipeDown ? 1 : -1)) % _rowCount;
                _currentItem = 0;
                _currentAction = 0;
                request = new JdChangeRowPhoneCommandData
                {
                    RowIndex = _currentRow
                };
                break;
            }
            case NavigationAction.SwipeLeft:
            case NavigationAction.SwipeRight:
            {
                if (_itemCount == 0)
                    break;
                
                _currentItem = (_itemCount + _currentItem + (action == NavigationAction.SwipeRight ? 1 : -1)) %
                               _itemCount;
                _currentAction = 0;
                request = new JdChangeItemPhoneCommandData
                {
                    RowIndex = _currentRow,
                    ItemIndex = _currentItem
                };
                break;
            }
        }
        
        if (request != null)
            await SendAsync(request);
    }
}