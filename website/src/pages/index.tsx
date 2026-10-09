import type {ReactElement} from 'react';
import Layout from '@theme/Layout';
import Link from '@docusaurus/Link';
import {Card, CardGrid} from '@site/src/components/CardGrid';

export default function Home(): ReactElement {
  return (
    <Layout
      title="UI library for Avalonia"
      description="Documentation for HammerUI, a themeable and extensible UI library for Avalonia on .NET 10: themes, icons, docking, dialogs and controls.">
      <main className="container margin-vert--xl">
        <h1>HammerUI</h1>
        <p className="hero__subtitle">
          A themeable and extensible UI library for Avalonia: light and dark palettes with a runtime accent color, docking with floating
          windows, dialogs, toasts, line icons and controls for desktop tools.
        </p>
        <div className="margin-vert--lg">
          <Link className="button button--primary button--lg" to="/guide/getting-started">
            Get started
          </Link>
        </div>
        <CardGrid columns={3}>
          <Card title="Theming" icon="palette" to="/guide/theming">
            Light, dark and system modes, and an accent color that changes at runtime.
          </Card>
          <Card title="Docking" icon="layers" to="/guide/docking">
            Tab groups that drag to any edge, into another group or out into their own window.
          </Card>
          <Card title="Controls" icon="sliders" to="/guide/controls">
            Fields that scrub, property rows, color, curve and gradient editors, and more.
          </Card>
        </CardGrid>
      </main>
    </Layout>
  );
}
