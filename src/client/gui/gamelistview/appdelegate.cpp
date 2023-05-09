#include "appdelegate.h"
#include <QPainter>
#include <QPoint>
#include "appmodel.h"
#include "treeitem.h"

void AppDelegate::paint(QPainter *painter, const QStyleOptionViewItem &option, const QModelIndex &index) const
{
    if (index.data().canConvert<TreeItem*>()) {
        TreeItem *item = qvariant_cast<TreeItem*>(index.data());
        if (item->type == TreeItemType::k_ETreeItemTypeApp)
        {   
            App *app = qvariant_cast<App*>(item->value);

            QRect adjustedRect = QRect(option.rect);
            adjustedRect.adjust(-48, 0, 0, 0);

            if (option.state & QStyle::State_Selected) {
                painter->fillRect(adjustedRect, option.palette.highlight());
            } else {
                painter->fillRect(adjustedRect, option.palette.base());
            }
                

            int widthLeft = adjustedRect.size().width();

            QRect statusRect = QRect(adjustedRect.topLeft()+QPoint(4, 2), QSize(20, 20));
            widthLeft - statusRect.size().width();

            QRect iconRect = QRect(statusRect.topRight()+QPoint(4, 0), QSize(20, 20));
            widthLeft - iconRect.size().width();

            QRect titleRect = QRect(iconRect.topRight()+QPoint(4, 0), QSize(widthLeft, adjustedRect.height()));
            //painter->fillRect(statusRect, Qt::gray);

            if (!app->libraryAssets.icon.isNull()) {
                QPixmap pixmap = QPixmap::fromImage(app->libraryAssets.icon).scaled(iconRect.size(), Qt::AspectRatioMode::IgnoreAspectRatio, Qt::TransformationMode::SmoothTransformation);

                painter->drawPixmap(iconRect, pixmap);
                pixmap = QPixmap();
            }
            else
            {
                painter->fillRect(iconRect, Qt::darkGray);
            }

            painter->drawText(titleRect, QString::fromStdString(std::string(app->name)));
        } else if (item->type == TreeItemType::k_ETreeItemTypeCategory) 
        {
            QString name = qvariant_cast<QString>(item->value);
            if (option.state & QStyle::State_Selected)
            {
                painter->fillRect(option.rect, option.palette.highlight());
            } else {
                painter->fillRect(option.rect, Qt::darkGray);
            }
                
            painter->drawText(option.rect, name);
        }   
    }
    else
    {
        QStyledItemDelegate::paint(painter, option, index);
    }
}

QSize AppDelegate::sizeHint(const QStyleOptionViewItem &option, const QModelIndex &index) const 
{   
    QSize precalculatedSize = QStyledItemDelegate::sizeHint(option, index);
    if (index.data().canConvert<TreeItem *>())
    {
        return QSize(precalculatedSize.width(), 24);
    }

    return precalculatedSize;
};