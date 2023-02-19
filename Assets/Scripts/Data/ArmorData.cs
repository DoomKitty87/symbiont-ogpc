public class ArmorData
{

  public string name;
  public string nickName;
  public float baseDefense;
  public int id;

  public ArmorData(string inName) {
    name = inName;
    switch(name) {
      case "Flowerpot":
        id = 0;
        nickName = "Flowerpot";
        baseDefense = 2f;
        break;
    }
  }
}