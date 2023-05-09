#include "appmodel.h"
#include "appdelegate.h"
#include "../../interop/app.h"
#include "treeitem.h"
#include <QAbstractItemView>
#include <chrono>

AppModel::AppModel(QObject *parent) {
    this->rootItem = new TreeItem(nullptr, k_ETreeItemTypeRoot);
    tree.push_back(rootItem);

    this->rootItemFiltered = new TreeItem(nullptr, k_ETreeItemTypeRoot);
}

int AppModel::rowCount(const QModelIndex& parent) const
{
    TreeItem *parentItem;

    if (parent.column() > 0)
        return 0;
        
    if (!parent.isValid())
        parentItem = rootItemFiltered;
    else
        parentItem = static_cast<TreeItem*>(parent.internalPointer());

    return parentItem->childrenFlat.count();
}

int AppModel::columnCount(const QModelIndex& parent) const
{
    return 1;
}

QVariant AppModel::headerData(int section, Qt::Orientation orientation,
                               int role) const
{
    if (orientation == Qt::Horizontal && role == Qt::DisplayRole)
        return rootItemFiltered->value;

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
        parentItem = rootItemFiltered;
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

    if (parentItem == rootItemFiltered)
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

void AppModel::addApp(std::vector<std::string> categories, App *app, bool initial)
{   
    beginInsertRows(QModelIndex(), rowCount(), rowCount());
    
    // All apps need to have a category, so add one if none were provided
    if (categories.empty()) {
        categories.push_back(std::string("uncategorized"));
    }   

    for (auto &&category : categories)
    {
        TreeItem *tCategory;

        // Turn favorite into favorites
        if (category == "favorite")
        {
            category = "favorites";
        }

        // For the sorting to work properly, the categories need to be uppercase
        category = QString::fromStdString(category).toUpper().toStdString();

        if (!containsCategory(QString::fromStdString(category)))
        {
            TreeItem *newCategory = new TreeItem(rootItem, k_ETreeItemTypeCategory);
            newCategory->value = QString::fromStdString(category);
            newCategory->text = category;

            rootItem->AddChild(newCategory);
            
            tree.push_back(newCategory);
            tCategory = newCategory;
        }
        else
        {
            tCategory = findCategory(QString::fromStdString(category));
        }

        TreeItem *newApp = new TreeItem(tCategory, k_ETreeItemTypeApp);
        newApp->text = app->name;
        newApp->value = QVariant::fromValue<App *>(app);

        tCategory->AddChild(newApp);

        tree.push_back(newApp);
    }
    endInsertRows();

    if (!initial) {
        filter();
    }   
}

std::set<EAppType> AppModel::getTypeFilter() {
    return this->typeFilter;
}

std::string AppModel::getNameContainsFilter() {
    return this->nameContainsFilter;
}

AppState AppModel::getStateFilter() {
    return this->stateFilter;
}

void AppModel::clearTypeFilter() {
    this->typeFilter = std::set<EAppType>();
    filter();
}

void AppModel::clearNameContainsFilter() {
    this->nameContainsFilter = "";
    filter();
}

void AppModel::clearStateFilter() {
    this->stateFilter = AppState(k_EAppStateInvalid);
    filter();
}

void AppModel::setTypeFilter(std::set<EAppType> types) {
    this->typeFilter = types;
    filter();
}

void AppModel::setNameContainsFilter(std::string contains) {
    this->nameContainsFilter = contains;
    filter();
}


void AppModel::setStateFilter(AppState state) {
    this->stateFilter = state;
    filter();
}

// This function is a trainwreck
void AppModel::filter() {
    auto startTime = std::chrono::steady_clock::now();
    DEBUG_MSG << "[AppModel] Filtering begin" << std::endl;
    beginResetModel();
    bool hasTypeFilter = !this->typeFilter.empty();
    bool hasNameContainsFilter = !this->nameContainsFilter.empty();
    bool hasStateFilter = this->stateFilter.state != k_EAppStateInvalid;

    // Clear the previous tree
    for (auto &&x : treeFiltered)
    {
        delete x;
        x = nullptr;
    }
    treeFiltered.clear();
    rootItemFiltered->childrenFlat.clear();

    // Only filter TreeItemTypeApp:s
    std::vector<TreeItem *> filterableItems;
    for (auto &&i : this->tree)
    {
        if (i->type == k_ETreeItemTypeApp) {
            filterableItems.push_back(i);
        }
    }

    // Filter by type
    //TODO: this could probably be improved
    std::vector<TreeItem *> filteredItems;
    if (hasTypeFilter) {
        for (auto &&i : filterableItems)
        {
            if (typeFilter.contains(i->value.value<App*>()->type)) {
                filteredItems.push_back(i);
            }
        }
        
    } else {
        filteredItems = filterableItems;
    }

    // Filter by name contains
    std::vector<TreeItem *> filteredItems2;
    if (hasNameContainsFilter) {
        for (auto &&i : filteredItems)
        {
            std::string appName = QString::fromStdString(i->value.value<App *>()->name).toLower().toStdString();
            std::string nameFilter = QString::fromStdString(nameContainsFilter).toLower().toStdString();
            if (appName.contains(nameFilter))
            {
                filteredItems2.push_back(i);
            }
        }
        
    } else {
        filteredItems2 = filteredItems;
    }

    std::vector<TreeItem *> filteredItems3;

    // Filter by state
    // State filtering is a narrowing operation
    // This means when you have multiple states checked, an app needs to match all those
    if (hasStateFilter) {
        for (auto &&i : filteredItems2)
        {
            AppState *appState = i->value.value<App *>()->state;
            bool pass = true;

            // This is not ideal....
            if (pass && stateFilter.Uninstalled) {
                pass = appState->Uninstalled;
            }
            if (pass && stateFilter.UpdateRequired) {
                pass = appState->UpdateRequired;
            }
            if (pass && stateFilter.FullyInstalled) {
                pass = appState->FullyInstalled;
            }
            if (pass && stateFilter.DataEncrypted) {
                pass = appState->DataEncrypted;
            }
            if (pass && stateFilter.SharedOnly) {
                pass = appState->SharedOnly;
            }
            if (pass && stateFilter.DataLocked) {
                pass = appState->DataLocked;
            }
            if (pass && stateFilter.FilesMissing) {
                pass = appState->FilesMissing;
            }
            if (pass && stateFilter.FilesCorrupt) {
                pass = appState->FilesCorrupt;
            }
            if (pass && stateFilter.AppRunning) {
                pass = appState->AppRunning;
            }
            if (pass && stateFilter.UpdateRunning) {
                pass = appState->UpdateRunning;
            }
            if (pass && stateFilter.UpdateStopping) {
                pass = appState->UpdateStopping;
            }
            if (pass && stateFilter.UpdatePaused) {
                pass = appState->UpdatePaused;
            }
            if (pass && stateFilter.UpdateStarted) {
                pass = appState->UpdateStarted;
            }
            if (pass && stateFilter.Reconfiguring) {
                pass = appState->Reconfiguring;
            }
            if (pass && stateFilter.AddingFiles) {
                pass = appState->AddingFiles;
            }
            if (pass && stateFilter.Downloading) {
                pass = appState->Downloading;
            }
            if (pass && stateFilter.Staging) {
                pass = appState->Staging;
            }
            if (pass && stateFilter.Committing) {
                pass = appState->Committing;
            }
            if (pass && stateFilter.Uninstalling) {
                pass = appState->Uninstalling;
            }
            if (pass && stateFilter.Preallocating) {
                pass = appState->Preallocating;
            }
            if (pass && stateFilter.Validating) {
                pass = appState->Validating;
            }

            if (pass) {
                filteredItems3.push_back(i);
            }
        }
    } else {
        filteredItems3 = filteredItems2;
    }

    // Copy all categories from the original tree
    std::vector<TreeItem *> categories;
    for (auto &&i : tree)
    {
        if (i->type == k_ETreeItemTypeCategory) {
            TreeItem *item = new TreeItem(rootItemFiltered, k_ETreeItemTypeCategory);
            item->text = i->text;
            item->value = i->value;
            categories.push_back(item);
            treeFiltered.push_back(item);
        }
    }

    // Add all the filtered apps to the categories and the tree
    for (auto &&i : filteredItems3)
    {
        if (i->type == k_ETreeItemTypeApp) {
            TreeItem *appCategory = nullptr;
            for (auto &&x : categories)
            {
                assert(i->parent->text != "");
                assert(x->text != "");
                if (i->parent->text == x->text)
                {
                    appCategory = x;
                }
            }

            assert(appCategory != nullptr);
            
            TreeItem *appItem = new TreeItem(appCategory, k_ETreeItemTypeApp);
            appItem->value = i->value;
            appItem->text = i->text;
            treeFiltered.push_back(appItem);
        }
    }

    endResetModel();
    DEBUG_MSG << "[AppModel] Filtering end, took " << (std::chrono::steady_clock::now() - startTime).count() << "ms" << std::endl;
    emit sortingFinishedd();
}