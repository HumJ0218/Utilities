# Utilities
各种常用的工具类、扩展方法

--------

# 位运算

- `byte.Bit_Reverse` 高低位倒转
- `ushort.Bit_Reverse` 高低位倒转
- `uint.Bit_Reverse` 高低位倒转
- `ulong.Bit_Reverse` 高低位倒转

# 字节数组

- `byte[].ByteArray_ToHexString` 字节数组转字符串

# 度分秒

- `string.DMS_Convert` 度分秒字符串转小数角度
- `double.DMS_Convert` 小数角度转度分秒字符串

# Excel 相关

- `int.Excel_ConvertToColumnLetter` 列序号数字转字符串
- `string.Excel_ConvertToColumnNumber` 列序号字符串转数字

# GZip 相关

- `byte[].GZip_Compress` 使用 GZip 压缩字节数组
- `byte[].GZip_Decompress` 使用 GZip 解压缩字节数组
- `string.GZip_CompressFromString` 使用 GZip 压缩字符串
- `byte[].GZip_DecompressToString` 使用 GZip 解压缩字符串

# 集合相关

- `IEnumerable<T>.IEnumerableT_IsNullOrEmpty` 返回集合是否为空或空集合
- `IEnumerable<T>.IEnumerableT_Jaccard` 计算与另一集合之间的 Jaccard 相似度
- `IEnumerable<T>.IEnumerableT_SorensenDice` 计算与另一集合之间的 Sorensen Dice 相似度
- `IEnumerable<T>.IEnumerableT_Levenshtein` 计算与另一集合之间的 Levenshtein 相似度
- `IEnumerable<T>.IEnumerableT_EditDistance` 计算与另一集合之间的编辑距离
- `IEnumerable<T>.IEnumerableT_Hamming` 计算与另一集合之间的 Hamming 相似度
- `IEnumerable<T>.IEnumerableT_Cosine` 计算与另一集合之间的余弦相似度
- `IEnumerable<T>.IEnumerableT_EuclideanDistance` 计算与另一集合之间的欧几里得距离指数
- `IEnumerable<T>.IEnumerableT_ManhattanDistance` 计算与另一集合之间的曼哈顿距离指数

# MD5 相关

- `byte[].MD5_ComputeHash` 计算字节数组的 MD5
- `Stream.MD5_ComputeHash` 计算字节流的 MD5
- `string.MD5_ComputeHash` 计算字符串的 MD5
- `FileInfo.MD5_ComputeHash` 计算文件的 MD5
