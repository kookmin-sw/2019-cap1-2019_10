﻿using System;
using Newtonsoft.Json;

[Serializable]
public class Result
{
    // json 파일 형식
    //public string id { get; set; }
    public string music { get; set; }
    public string link { get; set; }
    public string tag_1 { get; set; }
    public string tag_2 { get; set; }
}
