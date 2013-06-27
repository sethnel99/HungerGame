using System.Collections;

public abstract class EquipmentItem : Item {
    public enum EquipmentType {
        equipable, secondary_equipable, jacket, boots
    }
    public EquipmentType equipmentType;
    public float statPower;

    public EquipmentItem(string n, string pn, int w, int q)
        : base(n, pn, w, q) {
    }

    public EquipmentItem(EquipmentItem other)
        : base(other) {
            equipmentType = other.equipmentType;
            statPower = other.statPower;
    }

	

}
