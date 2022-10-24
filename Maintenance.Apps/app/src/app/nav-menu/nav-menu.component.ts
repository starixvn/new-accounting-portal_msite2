import { Component, OnInit } from '@angular/core';
import { NavMenuService } from './nav-menu.service';
import { Menu } from './nav-menu.model';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit {
  public menus: Menu[] | null = null;
  public userName!: string;
  constructor(private menuService: NavMenuService) {}

  ngOnInit(): void {
    this.loadMenu();
  }

  getUrl(validUrl: string) {
    return validUrl.split("/").pop()
  }

  loadMenu() {
    this.menuService.getMenu().subscribe((data) => {
      var listMenus = data.menus;
      var menuList = listMenus
        .filter((m) => m.parentId == 0)
        .sort((x) => x.displayOrder);

      menuList.forEach((menu) => {
        var subMenus = listMenus
          .filter((x) => x.parentId == menu.id)
          .sort((x) => x.displayOrder);
        menu.subMenu = subMenus;
      });

      this.menus = menuList;
      this.userName = data.userName;
    });
  }
}
