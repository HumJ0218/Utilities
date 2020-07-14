using System;
using System.Collections.Generic;
using System.Linq;

namespace HumJ.Utilities
{
    /// <summary>
    /// 对 IEnumerable T 类的扩展
    /// </summary>
    public static class IEnumerableT
    {
        /// <summary>
        /// 返回集合是否为空或空集合
        /// </summary>
        public static bool IEnumerableT_IsNullOrEmpty<T>(this IEnumerable<T> a)
        {
            return a == null || a.Count() == 0;
        }

        /// <summary>
        /// 计算与另一集合之间的 Jaccard 相似度（集合的交集数量与并集数量的比值）
        /// </summary>
        public static float IEnumerableT_Jaccard<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // 都为空，相似度为 1
            if (a.IEnumerableT_IsNullOrEmpty() && b.IEnumerableT_IsNullOrEmpty())
            {
                return 1;
            }

            // 一个空一个非空，相似度为 0
            if (a.IEnumerableT_IsNullOrEmpty() || b.IEnumerableT_IsNullOrEmpty())
            {
                return 0;
            }

            // 交集数量
            var intersection = a.Intersect(b).Count();

            // 并集数量
            var union = a.Union(b).Count();

            // 相似度
            return (float)intersection / union;
        }

        /// <summary>
        /// 计算与另一集合之间的 Sorensen Dice 相似度（集合的交集数量的 2 倍与总元素数量的比值）
        /// </summary>
        public static float IEnumerableT_SorensenDice<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // 都为空，相似度为 1
            if (a.IEnumerableT_IsNullOrEmpty() && b.IEnumerableT_IsNullOrEmpty())
            {
                return 1;
            }

            // 一个空一个非空，相似度为 0
            if (a.IEnumerableT_IsNullOrEmpty() || b.IEnumerableT_IsNullOrEmpty())
            {
                return 0;
            }

            // 交集数量
            var intersection = a.Intersect(b).Count();

            // 总元素数量
            var allcount = a.Count() + b.Count();

            // 相似度
            return (float)intersection * 2 / allcount;
        }

        /// <summary>
        /// 计算与另一集合之间的 Levenshtein 相似度（1 与编辑距离与较大集合元素数的比值的差）
        /// </summary>
        public static float IEnumerableT_Levenshtein<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // 都为空，相似度为 1
            if (a.IEnumerableT_IsNullOrEmpty() && b.IEnumerableT_IsNullOrEmpty())
            {
                return 1;
            }

            // 一个空一个非空，相似度为 0
            if (a.IEnumerableT_IsNullOrEmpty() || b.IEnumerableT_IsNullOrEmpty())
            {
                return 0;
            }

            // 相似度
            return 1 - (float)IEnumerableT_EditDistance(a, b) / Math.Max(a.Count(), b.Count());
        }

        /// <summary>
        /// 计算与另一集合之间的编辑距离（由一个集合变为另一个集合的最小操作次数）
        /// </summary>
        public static int IEnumerableT_EditDistance<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            var aLen = a.Count();
            var bLen = b.Count();

            if (aLen == 0) return aLen;
            if (bLen == 0) return bLen;

            var aa = a.ToArray();
            var bb = b.ToArray();

            var v = new int[aLen + 1, bLen + 1];
            for (var i = 0; i <= aLen; ++i)
            {
                for (var j = 0; j <= bLen; ++j)
                {
                    if (i == 0)
                    {
                        v[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        v[i, j] = i;
                    }
                    else if (aa[i - 1].Equals(bb[j - 1]))
                    {
                        v[i, j] = v[i - 1, j - 1];
                    }
                    else
                    {
                        v[i, j] = 1 + Math.Min(v[i - 1, j - 1], Math.Min(v[i, j - 1], v[i - 1, j]));
                    }
                }
            }
            return v[aLen, bLen];
        }

        /// <summary>
        /// 计算与另一集合之间的 Hamming 相似度（等元素数集合中不一致的元素个数的占比）
        /// </summary>
        public static float IEnumerableT_Hamming<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // 都为空，相似度为 1
            if (a.IEnumerableT_IsNullOrEmpty() && b.IEnumerableT_IsNullOrEmpty())
            {
                return 1;
            }

            // 一个空一个非空，相似度为 0
            if (a.IEnumerableT_IsNullOrEmpty() || b.IEnumerableT_IsNullOrEmpty())
            {
                return 0;
            }

            var aa = a.ToArray();
            var bb = b.ToArray();

            // 长度不一致，相似度为 0
            if (aa.Length != bb.Length)
            {
                return 0;
            }

            var differenceCount = 0f;
            for (int i = 0; i < aa.Length; i++)
            {
                if (aa[i].Equals(bb[i]))
                {
                    differenceCount++;
                }
            }
            return differenceCount / aa.Length;
        }

        /// <summary>
        /// 计算与另一集合之间的余弦相似度（向量夹角的余弦值）<br/>
        /// 化为向量的规则是，将并集作为向量空间，每一个元素作为一个维度，维度的数值为对应元素出现的频率
        /// </summary>
        public static float IEnumerableT_Cosine<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // 都为空，相似度为 1
            if (a.IEnumerableT_IsNullOrEmpty() && b.IEnumerableT_IsNullOrEmpty())
            {
                return 1;
            }

            // 一个空一个非空，相似度为 0
            if (a.IEnumerableT_IsNullOrEmpty() || b.IEnumerableT_IsNullOrEmpty())
            {
                return 0;
            }

            // 向量化
            var union = a.Union(b);
            var vectorA = union.ToDictionary(d => d, d => a.Count(c => c.Equals(d)));
            var vectorB = union.ToDictionary(d => d, d => b.Count(c => c.Equals(d)));

            // 内积
            var innerProduct = union.Sum(d => vectorA[d] * vectorB[d]);

            // 向量模长
            var moldA = Math.Sqrt(vectorA.Values.Sum(d => d * d));
            var moldB = Math.Sqrt(vectorB.Values.Sum(d => d * d));

            // 夹角余弦值
            var cosine = innerProduct / (moldA * moldB);

            return (float)cosine;
        }

        /// <summary>
        /// 计算与另一集合之间的欧几里得距离指数（向量差的模长）<br/>
        /// 化为向量的规则是，将并集作为向量空间，每一个元素作为一个维度，维度的数值为对应元素出现的频率<br/>
        /// 距离指数 = 1 / (距离 + 1)
        /// </summary>
        public static float IEnumerableT_EuclideanDistance<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // 都为空，相似度为 1
            if (a.IEnumerableT_IsNullOrEmpty() && b.IEnumerableT_IsNullOrEmpty())
            {
                return 1;
            }

            // 一个空一个非空，相似度为 0
            if (a.IEnumerableT_IsNullOrEmpty() || b.IEnumerableT_IsNullOrEmpty())
            {
                return 0;
            }

            // 向量化
            var union = a.Union(b);
            var vectorA = union.ToDictionary(d => d, d => a.Count(c => c.Equals(d)));
            var vectorB = union.ToDictionary(d => d, d => b.Count(c => c.Equals(d)));

            // 差向量
            var difference = union.ToDictionary(d => d, d => vectorA[d] - vectorB[d]);

            // 向量模长
            var mold = Math.Sqrt(difference.Values.Sum(d => d * d));

            // 距离指数
            var distance = 1d / (mold + 1);
            return (float)distance;
        }

        /// <summary>
        /// 计算与另一集合之间的曼哈顿距离指数（向量差的维度值总和）<br/>
        /// 化为向量的规则是，将并集作为向量空间，每一个元素作为一个维度，维度的数值为对应元素出现的频率<br/>
        /// 距离指数 = 1 / (距离 + 1)
        /// </summary>
        public static float IEnumerableT_ManhattanDistance<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            // 都为空，相似度为 1
            if (a.IEnumerableT_IsNullOrEmpty() && b.IEnumerableT_IsNullOrEmpty())
            {
                return 1;
            }

            // 一个空一个非空，相似度为 0
            if (a.IEnumerableT_IsNullOrEmpty() || b.IEnumerableT_IsNullOrEmpty())
            {
                return 0;
            }

            // 向量化
            var union = a.Union(b);
            var vectorA = union.ToDictionary(d => d, d => a.Count(c => c.Equals(d)));
            var vectorB = union.ToDictionary(d => d, d => b.Count(c => c.Equals(d)));

            // 差向量
            var difference = union.ToDictionary(d => d, d => vectorA[d] - vectorB[d]);

            // 维度和
            var sum = union.Sum(d => difference[d]);

            // 距离指数
            var distance = 1d / (sum + 1);
            return (float)distance;
        }
    }
}