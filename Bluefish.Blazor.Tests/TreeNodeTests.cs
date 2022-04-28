using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluefish.Blazor.Tests
{
    [TestClass]
    public class TreeNodeTests
    {

        [TestMethod]
        public void ToString_ReturnsPath()
        {
            // arrange
            var root = new TreeNode<FileSystemEntry>(new FileSystemEntry("root"));

            // act
            var node1 = root.AddNode(new FileSystemEntry("a"));
            var node2 = node1.AddNode(new FileSystemEntry("b"));

            // assert
            Assert.AreEqual("/root/a/b", node2.ToString());
        }

        [TestMethod]
        public void AddNode_BuildsTreeOfInt()
        {
            // arrange
            var root = new TreeNode<int>(1);

            // act
            var node1 = root.AddNode(2);

            // assert
            Assert.AreEqual(1, root.Item);
            Assert.AreEqual(2, node1.Item);
            Assert.AreEqual("/1/2", node1.GetPath());
        }

        [TestMethod]
        public void AddNode_BuildsTreeStructure()
        {
            // arrange
            var root = new TreeNode<FileSystemEntry>(new FileSystemEntry("root"));

            // act
            var node1 = root.AddNode(new FileSystemEntry("a") );
            root.AddNode(new FileSystemEntry("b"));
            node1.AddNode(new FileSystemEntry("c"));

            // assert
            Assert.AreEqual(2, root.ChildNodes.Length);
            Assert.AreEqual(1, root.ChildNodes[0].ChildNodes.Length);
        }

        [TestMethod]
        public void AddNode_SetsParent()
        {
            // arrange
            var root = new TreeNode<FileSystemEntry>(new FileSystemEntry("root"));

            // act
            var node = root.AddNode(new FileSystemEntry("a"));

            // assert
            Assert.AreSame(root, node.Parent);
        }

        [TestMethod]
        public void GetStack_ReturnsPopulatedStack()
        {
            // arrange
            var root = new TreeNode<FileSystemEntry>(new FileSystemEntry("root"));
            var node1 = root.AddNode(new FileSystemEntry("a"));
            var node2 = node1.AddNode(new FileSystemEntry("b"));
            var node3 = node2.AddNode(new FileSystemEntry("c"));
            var node4 = node3.AddNode(new FileSystemEntry("d"));
            var node5 = root.AddNode(new FileSystemEntry("x"));
            var node6 = node5.AddNode(new FileSystemEntry("y"));
            node6.AddNode(new FileSystemEntry("z"));

            // act
            var stackArray = node4.GetNodeStack().ToArray();

            // assert
            Assert.IsNotNull(stackArray);
            Assert.AreEqual(5, stackArray.Length);
            Assert.AreSame(root, stackArray[0]);
            Assert.AreSame(node1, stackArray[1]);
            Assert.AreSame(node2, stackArray[2]);
            Assert.AreSame(node3, stackArray[3]);
            Assert.AreSame(node4, stackArray[4]);
        }

        [TestMethod]
        public void GetPath_ReturnsFullPath()
        {
            // arrange
            var root = new TreeNode<FileSystemEntry>(new FileSystemEntry("root"));
            var node1 = root.AddNode(new FileSystemEntry("a"));
            root.AddNode(new FileSystemEntry("x"));
            var node2 = node1.AddNode(new FileSystemEntry("b"));
            var node3 = node2.AddNode(new FileSystemEntry("c"));

            // act
            var path = node3.GetPath();

            // assert
            Assert.AreEqual("/root/a/b/c", path);
        }

        [TestMethod]
        public void GetSiblings_ReturnsSiblingsIncludingSelf()
        {
            // arrange
            var root = new TreeNode<int>(0);
            var node1 = root.AddNode(1);
            root.AddNode(2);
            var node3 = node1.AddNode(3);
            var node4 = node3.AddNode(4);
            var node5 = node3.AddNode(5);
            var node6 = node3.AddNode(6);

            // act
            var siblings = node5.GetSiblings(true);

            // assert
            Assert.IsNotNull(siblings);
            Assert.AreEqual(3, siblings.Length);
            CollectionAssert.AreEquivalent(new[] { node4, node5, node6 }, siblings);
        }

        [TestMethod]
        public void GetSiblings_ReturnsSiblingsExcludingSelf()
        {
            // arrange
            var root = new TreeNode<int>(0);
            var node1 = root.AddNode(1);
            root.AddNode(2);
            var node3 = node1.AddNode(3);
            var node4 = node3.AddNode(4);
            var node5 = node3.AddNode(5);
            var node6 = node3.AddNode(6);

            // act
            var siblings = node5.GetSiblings(false);

            // assert
            Assert.IsNotNull(siblings);
            Assert.AreEqual(2, siblings.Length);
            CollectionAssert.AreEquivalent(new[] { node4, node6 }, siblings);
        }

        [TestMethod]
        public void GetSiblings_RootReturnsEmptyArray()
        {
            // arrange
            var root = new TreeNode<int>(0);
            var node1 = root.AddNode(1);
            root.AddNode(2);
            var node3 = node1.AddNode(3);
            var node4 = node3.AddNode(4);
            var node5 = node3.AddNode(5);
            var node6 = node3.AddNode(6);

            // act
            var siblings = node5.GetSiblings(false);

            // assert
            Assert.IsNotNull(siblings);
            Assert.AreEqual(2, siblings.Length);
            CollectionAssert.AreEquivalent(new[] { node4, node6 }, siblings);
        }
    }
}