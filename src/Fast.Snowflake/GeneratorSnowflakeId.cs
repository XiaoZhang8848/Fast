using Fast.DependencyInjection;

namespace Fast.Snowflake;

public class GeneratorSnowflakeId : IGeneratorSnowflakeId, ISingletonTag
{
    //基准时间
    private const long BASE_TIME = 1288834974657;

    //机器标识位数
    private const int WORKER_ID_BITS = 5;

    //数据标志位数
    private const int DATACENTER_ID_BITS = 5;

    //序列号识位数
    private const int SEQUENCE_BITS = 12;

    //序列号ID最大值
    private const long SEQUENCE_MASK = -1 ^ (-1 << SEQUENCE_BITS);

    //机器ID偏左移12位
    private const int WORKER_ID_SHIFT = SEQUENCE_BITS;

    //数据ID偏左移17位
    private const int DATACENTER_ID_SHIFT = SEQUENCE_BITS + WORKER_ID_BITS;

    //时间毫秒左移22位
    private const int TIMESTAMP_LEFT_SHIFT = SEQUENCE_BITS + WORKER_ID_BITS + DATACENTER_ID_BITS;

    // 工作机器ID
    private const long WORKER_ID = 1;

    // 数据中心ID
    private const long DATACENTER_ID = 1;

    // 锁
    private readonly object _lock = new();

    private long _lastTimestamp = -1;
    private long _sequence;

    public long GenerateId()
    {
        lock (_lock)
        {
            var timestamp = TimeGen();
            if (timestamp < _lastTimestamp)
            {
                throw new Exception($"时间戳必须大于上一次生成ID的时间戳.  拒绝为{_lastTimestamp - timestamp}毫秒生成id");
            }

            //如果上次生成时间和当前时间相同,在同一毫秒内
            if (_lastTimestamp == timestamp)
            {
                //sequence自增，和sequenceMask相与一下，去掉高位
                _sequence = (_sequence + 1) & SEQUENCE_MASK;
                //判断是否溢出,也就是每毫秒内超过1024，当为1024时，与sequenceMask相与，sequence就等于0
                if (_sequence == 0)
                {
                    //等待到下一毫秒
                    timestamp = TilNextMillis(_lastTimestamp);
                }
            }
            else
            {
                //如果和上次生成时间不同,重置sequence，就是下一毫秒开始，sequence计数重新从0开始累加,
                //为了保证尾数随机性更大一些,最后一位可以设置一个随机数
                _sequence = 0; //new Random().Next(10);
            }

            _lastTimestamp = timestamp;
            return ((timestamp - BASE_TIME) << TIMESTAMP_LEFT_SHIFT) | (DATACENTER_ID << DATACENTER_ID_SHIFT) |
                   (WORKER_ID << WORKER_ID_SHIFT) | _sequence;
        }
    }

    // 防止产生的时间比之前的时间还要小（由于NTP回拨等问题）,保持增量的趋势.
    private long TilNextMillis(long lastTimestamp)
    {
        var timestamp = TimeGen();
        while (timestamp <= lastTimestamp)
        {
            timestamp = TimeGen();
        }

        return timestamp;
    }

    // 获取当前的时间戳
    private long TimeGen()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}