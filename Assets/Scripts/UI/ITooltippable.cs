
public interface ITooltippable
{
    /**
     * Get header text of a tooltip for entity this script is  attached to
     *
     * <returns>Tooltip header string</returns>
     */
    public string GetTooltipHeader();
    /**
     * Get description text of a tooltip for entity this script is  attached to
     *
     * <returns>Tooltip body string</returns>
     */
    public string GetTooltipDesc();
}
