import { Component, OnInit } from '@angular/core';
import { NavMenuService } from './nav-menu.service';
import { Menu, Header } from './nav-menu.model';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit {
  public menus: Menu[] | null = null;
  public userName!: string;
  constructor(private menuService: NavMenuService) { }

  ngOnInit(): void {
    this.loadMenu();
  }

  getUrl(validUrl: string) {
    var url = validUrl.split('/');
    url.shift();
    return url.slice(1).join('/');
  }

  loadMenu() {
    this.menuService.getMenu().subscribe((data) => {
      const listMenus = [...data.menus];

      const menuList = listMenus
        .filter((m) => m.parentId === 0)
        .sort((x) => x.displayOrder);

      menuList.forEach((menu) => {
        const subMenus = listMenus
          .filter((x) => x.parentId === menu.id)
          .sort((x) => x.displayOrder);
        menu.subMenu = subMenus;
      });

      menuList.forEach((menu) => {
        menu.subMenu.forEach((m) => {
          const subMenus = listMenus
            .filter((x) => x.parentId === m.id)
            .sort((x) => x.displayOrder);
          m.subMenu = subMenus;
        });
      });

      this.menus = [...menuList];
      this.userName = data.userName;
    });
  }
}
