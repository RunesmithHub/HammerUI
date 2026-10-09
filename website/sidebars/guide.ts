import type {SidebarsConfig} from '@docusaurus/plugin-content-docs';

const sidebars: SidebarsConfig = {
  guide: [
    'index',
    'getting-started',
    'theming',
    'icons',
    'docking',
    'dialogs-and-toasts',
    'shell-styles',
    {
      type: 'category',
      label: 'Controls',
      link: {type: 'doc', id: 'controls/index'},
      items: [
        'controls/buttons',
        'controls/fields',
        'controls/properties',
        'controls/colors',
        'controls/curves-and-gradients',
        'controls/feedback',
        'controls/layout',
        'controls/data',
      ],
    },
  ],
};

export default sidebars;
