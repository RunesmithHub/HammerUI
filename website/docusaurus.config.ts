import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';
import {themes as prismThemes} from 'prism-react-renderer';

const github = 'https://github.com/RunesmithHub/HammerUI';

const config: Config = {
  title: 'HammerUI',
  tagline: 'A themeable, extensible UI library for Avalonia.',
  favicon: 'img/favicon.svg',
  url: process.env.SITE_URL || 'https://hammerui.runesmith.dev',
  baseUrl: process.env.BASE_URL || '/',
  trailingSlash: false,
  onBrokenLinks: 'throw',
  onBrokenAnchors: 'throw',

  future: {
    v4: true,
    faster: true,
  },

  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },

  markdown: {
    hooks: {
      onBrokenMarkdownLinks: 'throw',
    },
  },

  themes: [
    [
      '@easyops-cn/docusaurus-search-local',
      {
        hashed: true,
        docsDir: ['content/guide', 'content/developers'],
        docsRouteBasePath: ['guide', 'developers'],
        docsPluginIdForPreferredVersion: 'default',
        indexBlog: false,
        highlightSearchTermsOnTargetPage: true,
        explicitSearchResultPath: true,
        searchBarShortcutHint: true,
      },
    ],
  ],

  presets: [
    [
      'classic',
      {
        docs: {
          path: 'content/guide',
          routeBasePath: 'guide',
          sidebarPath: './sidebars/guide.ts',
        },
        blog: false,
        theme: {
          customCss: './src/css/custom.css',
        },
        sitemap: {
          changefreq: 'weekly',
          priority: 0.5,
        },
      } satisfies Preset.Options,
    ],
  ],

  plugins: [
    [
      '@docusaurus/plugin-content-docs',
      {
        id: 'developers',
        path: 'content/developers',
        routeBasePath: 'developers',
        sidebarPath: './sidebars/developers.ts',
      },
    ],
  ],

  headTags: [{tagName: 'meta', attributes: {name: 'theme-color', content: '#5B5BEF'}}],

  themeConfig: {
    colorMode: {
      defaultMode: 'dark',
      respectPrefersColorScheme: true,
    },
    docs: {
      sidebar: {
        hideable: true,
        autoCollapseCategories: true,
      },
    },
    tableOfContents: {
      minHeadingLevel: 2,
      maxHeadingLevel: 3,
    },
    navbar: {
      title: 'HammerUI',
      hideOnScroll: false,
      logo: {
        alt: 'HammerUI',
        src: 'img/logo.svg',
      },
      items: [
        {type: 'docSidebar', sidebarId: 'guide', position: 'left', label: 'Guide'},
        {type: 'docSidebar', sidebarId: 'developers', docsPluginId: 'developers', position: 'left', label: 'Developers'},
        {href: github, label: 'GitHub', position: 'right'},
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Guide',
          items: [
            {label: 'Introduction', to: '/guide'},
            {label: 'Getting started', to: '/guide/getting-started'},
            {label: 'Theming', to: '/guide/theming'},
            {label: 'Docking', to: '/guide/docking'},
          ],
        },
        {
          title: 'Controls',
          items: [
            {label: 'Buttons and badges', to: '/guide/controls/buttons'},
            {label: 'Inputs and fields', to: '/guide/controls/fields'},
            {label: 'Properties', to: '/guide/controls/properties'},
            {label: 'Colors', to: '/guide/controls/colors'},
          ],
        },
        {
          title: 'More',
          items: [
            {label: 'Building and testing', to: '/developers/building'},
            {label: 'Contributing', to: '/developers/contributing'},
            {label: 'GitHub', href: github},
          ],
        },
      ],
      copyright: `Copyright ${new Date().getFullYear()} Dylan de Beer. Built with Docusaurus.`,
    },
    prism: {
      theme: prismThemes.oneLight,
      darkTheme: prismThemes.oneDark,
      additionalLanguages: ['csharp', 'bash', 'json'],
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
