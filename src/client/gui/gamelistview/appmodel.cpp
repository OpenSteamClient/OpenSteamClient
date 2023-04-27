#include "appmodel.h"
#include "appdelegate.h"
#include "../../interop/app.h"
#include "treeitem.h"
#include <QAbstractItemView>

AppModel::AppModel(QObject *parent) {
    this->rootItem = new TreeItem();
    this->rootItem->type = TreeItemType::k_ETreeItemTypeRoot;
    this->rootItem->parent = nullptr;
}

int AppModel::rowCount(const QModelIndex& parent) const
{
    TreeItem *parentItem;

    if (parent.column() > 0)
        return 0;
        
    if (!parent.isValid())
        parentItem = rootItem;
    else
        parentItem = static_cast<TreeItem*>(parent.internalPointer());

    return parentItem->children.count();
}

int AppModel::columnCount(const QModelIndex& parent) const
{
    return 1;
}

QVariant AppModel::headerData(int section, Qt::Orientation orientation,
                               int role) const
{
    if (orientation == Qt::Horizontal && role == Qt::DisplayRole)
        return rootItem->value;

    return QVariant();
}

QVariant AppModel::data(const QModelIndex &index, int role) const 
{
    if (!index.isValid()) {
        return QVariant();
    }
       

    return QVariant::fromValue(static_cast<TreeItem*>(index.internalPointer()));
}

Qt::ItemFlags AppModel::flags(const QModelIndex &index) const 
{
   return Qt::ItemFlags(Qt::ItemFlag::ItemIsEnabled | Qt::ItemFlag::ItemIsSelectable);
}

QModelIndex AppModel::index(int row, int column, const QModelIndex &parent) const 
{
    if (!hasIndex(row, column, parent))
        return QModelIndex();

    TreeItem *parentItem;

    if (!parent.isValid())
        parentItem = rootItem;
    else
        parentItem = static_cast<TreeItem*>(parent.internalPointer());

    TreeItem *childItem = parentItem->getChildByRow(row);

    if (childItem)
        return createIndex(row, column, childItem);

    return QModelIndex();
}

QModelIndex AppModel::parent(const QModelIndex &index) const 
{
    if (!index.isValid())
        return QModelIndex();

    TreeItem *childItem = static_cast<TreeItem*>(index.internalPointer());
    TreeItem *parentItem = childItem->parent;

    if (parentItem == rootItem)
        return QModelIndex();

    return createIndex(parentItem->row(), 0, parentItem);
}

bool AppModel::containsCategory(QString category) {
   return findCategory(category) != nullptr;
}

TreeItem *AppModel::findCategory(QString category) {
    // NOTE: this is very inefficient, but it's not a problem (yet)
    for (auto &&i : tree)
    {
        if (i->type == TreeItemType::k_ETreeItemTypeCategory) {
            QString catName = qvariant_cast<QString>(i->value);
            if (category == catName) {
                return i;
            }
        }
    }

    return nullptr;
}

void AppModel::addApp(std::vector<std::string> categories, App *app)
{
    beginInsertRows(QModelIndex(), rowCount(), rowCount());
    
    // All apps need to have a category, so add one if none were provided
    if (categories.empty()) {
        categories.push_back(std::string("Uncategorized"));
    }

    for (auto &&category : categories)
    {
        TreeItem *tCategory;
        if (!containsCategory(QString::fromStdString(category))) {
            TreeItem *newCategory = new TreeItem();
            newCategory->value = QString::fromStdString(category);
            newCategory->parent = rootItem;
            rootItem->children.append(newCategory);
            newCategory->type = TreeItemType::k_ETreeItemTypeCategory;
            tree.push_back(newCategory);
            tCategory = newCategory;
        }
        else
        {
            tCategory = findCategory(QString::fromStdString(category));
        }
        TreeItem *newApp = new TreeItem();
        newApp->type = TreeItemType::k_ETreeItemTypeApp;
        newApp->parent = tCategory;
        newApp->value = QVariant::fromValue<App *>(app);

        tCategory->children.append(newApp);

        tree.push_back(newApp);
    }
    
    endInsertRows();
}