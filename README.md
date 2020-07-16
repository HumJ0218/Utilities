# Utilities
各种常用的工具类、扩展方法

--------

# 位运算
- `byte.BitReverse` 高低位倒转
- `ushort.BitReverse` 高低位倒转
- `uint.BitReverse` 高低位倒转
- `ulong.BitReverse` 高低位倒转

# 字节数组
- `byte[].ToHexString` 字节数组转字符串

# 度分秒
- `string.ConvertDmsToDeg` 度分秒字符串转小数角度
- `double.ConvertDegToDms` 小数角度转度分秒字符串

# Excel
- `int.ConvertToExcelColumnLetter` 列序号数字转字符串
- `string.ConvertToExcelColumnNumber` 列序号字符串转数字

# GIS
- …… 坐标系变换 从略
- `(double, double).GetGisTileXY` 获取指定经纬度对应的地图瓦片序号

# 图形
- `(double, double).GetRotateRectangle` 计算矩形旋转任意角度后的宽高
- `Image.GetRotateImage` 获取旋转任意角度后的图像

# GZip
- `byte[].GZipCompress` 使用 GZip 压缩字节数组
- `byte[].GZipDecompress` 使用 GZip 解压缩字节数组
- `string.GZipCompressFromString` 使用 GZip 压缩字符串
- `byte[].GZipDecompressToString` 使用 GZip 解压缩字符串

# 集合
- `IEnumerable<T>.IsNullOrEmpty` 返回集合是否为空或空集合
- `IEnumerable<T>.Jaccard` 计算与另一集合之间的 Jaccard 相似度
- `IEnumerable<T>.SorensenDice` 计算与另一集合之间的 Sorensen Dice 相似度
- `IEnumerable<T>.Levenshtein` 计算与另一集合之间的 Levenshtein 相似度
- `IEnumerable<T>.EditDistance` 计算与另一集合之间的编辑距离
- `IEnumerable<T>.Hamming` 计算与另一集合之间的 Hamming 相似度
- `IEnumerable<T>.Cosine` 计算与另一集合之间的余弦相似度
- `IEnumerable<T>.EuclideanDistance` 计算与另一集合之间的欧几里得距离指数
- `IEnumerable<T>.ManhattanDistance` 计算与另一集合之间的曼哈顿距离指数

# MD5
- `byte[].GetMD5Hash` 计算字节数组的 MD5
- `Stream.GetMD5Hash` 计算字节流的 MD5
- `string.GetMD5Hash` 计算字符串的 MD5
- `FileInfo.GetMD5Hash` 计算文件的 MD5
