public enum StatusType { TODO };

public class Status
{
    /*
     * Base class for status effects.
     * Each status will be be a subclass of this class.
     * In general, statuses will modify the attributes
     * of the actor that they are applied to, but they
     * can possibly do other things too.
     */

    public StatusType type = StatusType.TODO;

}