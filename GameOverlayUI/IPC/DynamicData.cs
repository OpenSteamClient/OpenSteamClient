namespace GameOverlayUI.IPC;

public struct DynamicData {
    public byte[] DisplayData;
    public byte[] InputData;

    public DynamicData(ReadOnlySpan<byte> serialized, int displayDataLength) {
        DisplayData = serialized[0..displayDataLength].ToArray();
        InputData = serialized[displayDataLength..].ToArray();
    }

    public DynamicData(byte[] displayData, byte[] inputData) {
        this.DisplayData = displayData;
        this.InputData = inputData;
    }

    public byte[] Serialize() {
        byte[] buf = new byte[DisplayData.Length + InputData.Length];
        Buffer.BlockCopy(DisplayData, 0, buf, 0, DisplayData.Length);
        Buffer.BlockCopy(InputData, 0, buf, DisplayData.Length, InputData.Length);
        return buf;
    }
}