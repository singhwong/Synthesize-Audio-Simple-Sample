// See https://aka.ms/new-console-template for more information

using System.Xml.Linq;
using Microsoft.CognitiveServices.Speech;

 
await SynthesizeAudioAsync();
static async Task SynthesizeAudioAsync()
{
    var config = SpeechConfig.FromSubscription("your primary key", "eastasia");
    //config.SpeechSynthesisLanguage = "zh-CN";
    //config.SpeechSynthesisVoiceName = "zh-CN-XiaomoNeural";

    // 忧虑成年女声
    (string role, string name,string style, string rate) = ("YoungAdultFemale", "zh-CN-XiaomoNeural", "depressed", "slow");
    // 欢快热情女声
    (string role1, string name1, string style1, string rate1) = ("YoungAdultFemale", "zh-CN-XiaomoNeural", "cheerful", "slow");
    // 正文(冷静，语速默认)
    (string role2, string name2, string style2, string rate2) = ("YoungAdultFemale", "zh-CN-XiaomoNeural", "calm", "default");

    string content = " 落日镕金，暮云合璧，人在何处？染柳烟浓，吹梅笛怨，春意知几许！元宵佳节，融合天气，次第岂无风雨？来相召、香车宝马，谢他酒朋诗侣。\n        中州盛日，闺门多暇，记得偏重三五。铺翠冠儿，捻金雪柳，簇带争济楚。如今憔悴，风鬟雾鬓，怕见夜间出去。不如向、帘儿底下，听人笑语。";
    using var synthesizer = new SpeechSynthesizer(config);
    var result = GenerateSsm("zh-CN", role, name, content, style,rate);
    Console.WriteLine(result);
    //var ssml = File.ReadAllText("./ssml.xml");
    //var result = await synthesizer.SpeakSsmlAsync(ssml);
    //await synthesizer.SpeakTextAsync("寻寻觅觅，冷冷清清，凄凄惨惨戚戚。乍暖还寒时候，最难将息。三杯两盏淡酒，怎敌他、晚来风急？雁过也，正伤心，却是旧时相识。满地黄花堆积。憔悴损，如今有谁堪摘？守著窗儿，独自怎生得黑？梧桐更兼细雨，到黄昏、点点滴滴。这次第，怎一个愁字了得！");

    await synthesizer.SpeakSsmlAsync(result);
} 
static string GenerateSsm(string locale,string gender,string name,string text,string style,string rate)
{
    XNamespace mstts = "https://www.w3.org/2001/mstts";
    var ssmlDoc = new XDocument(
        new XElement(XNamespace.Get("http://www.w3.org/2001/10/synthesis")+"speak",
        new XAttribute(XNamespace.Xmlns+ "mstts", "https://www.w3.org/2001/mstts"),
        new XAttribute("version","1.0"),
        new XAttribute(XNamespace.Xml+"lang",locale),
        new XElement("voice",
        new XAttribute("name",name),
        new XElement(mstts+ "express-as",
        // 风格(https://docs.microsoft.com/zh-cn/azure/cognitive-services/speech-service/speech-synthesis-markup?tabs=csharp#adjust-speaking-styles)
        new XAttribute("style", style),
        new XAttribute("styledegree", "1"),
        new XAttribute("role", gender),
        new XElement("prosody",
        // 语速(https://docs.microsoft.com/zh-cn/azure/cognitive-services/speech-service/speech-synthesis-markup?tabs=csharp#adjust-prosody)
        new XAttribute("rate",rate),
        text)))));
    return ssmlDoc.ToString();
}
